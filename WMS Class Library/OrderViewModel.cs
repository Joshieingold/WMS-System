using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Npgsql;
using System.Windows.Input;
using WMS_Class_Library;

public class OrderViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Order> PendingOrders { get; set; } = new();
    public ObservableCollection<string> OrderDetails { get; set; } = new();

    private Order _selectedOrder;
    public Order SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            _selectedOrder = value;
            OnPropertyChanged();
            if (_selectedOrder != null)
                LoadOrderDetails(_selectedOrder.OrderId);
        }
    }

    public string LPN { get; set; }
    public string SerialInput { get; set; }

    private readonly string connectionString = "Host=localhost;Username=postgres;Password=Koreanishard2424$$;Database=postgres";

    public OrderViewModel()
    {
        LoadPendingOrders();
    }

    public void LoadPendingOrders()
    {
        PendingOrders.Clear();

        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        string query = "SELECT order_id, from_org, status FROM pending_orders WHERE status = 'PENDING' AND from_org = 'NB1'";

        using var cmd = new NpgsqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var order = new Order
            {
                OrderId = reader.GetInt32(0),
                FromOrg = reader.GetString(1),
                Status = reader.GetString(2)
            };

            PendingOrders.Add(order);
        }
    }

    public void LoadOrderDetails(int orderId)
    {
        OrderDetails.Clear();

        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        string query = @"
            SELECT d.sku, pod.quantity
            FROM pending_order_details pod
            JOIN devices d ON d.id = pod.device_id
            WHERE pod.order_id = @orderId";

        using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("orderId", orderId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string sku = reader.GetString(0);
            int qty = reader.GetInt32(1);
            OrderDetails.Add($"{sku}: {qty}");
        }
    }

    // INotifyPropertyChanged setup
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
