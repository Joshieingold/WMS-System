using Npgsql;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WMS_System.Views
{
    public partial class PickControl : UserControl
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=Koreanishard2424$$;Database=postgres";

        public PickControl()
        {
            InitializeComponent();
        }

        private void SerialInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string serial = SerialInputBox.Text.Trim();
                if (!string.IsNullOrEmpty(serial))
                {
                    var deviceData = GetDeviceData(serial);

                    if (deviceData.HasValue)
                    {
                        string subInventory = deviceData.Value.SubInventory;
                        string deviceSku = deviceData.Value.DeviceName;

                        AddSerialRow(serial, subInventory, deviceSku);
                        SerialInputBox.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Serial not found or error occurred.");
                    }
                }
            }
        }



        private (string SubInventory, string DeviceName)? GetDeviceData(string serial)
        {
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                    SELECT i.sub_inventory, d.sku 
                    FROM inventory i 
                    JOIN devices d ON i.device_id = d.id 
                    WHERE i.serial = @serial AND i.organization = 'NB1'", conn);

                cmd.Parameters.AddWithValue("serial", serial);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (
                        reader["sub_inventory"].ToString(),
                        reader["sku"].ToString()
                    );
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void AddSerialRow(string serial, string inventory, string device)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(58, 57, 82)),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(2),
                Margin = new Thickness(0, 5, 0, 5)
            };

            var rowPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var serialBox = new TextBox { Width = 200, Margin = new Thickness(5), Text = serial, IsReadOnly = true };
            var inventoryBox = new TextBox { Width = 150, Margin = new Thickness(5), Text = inventory, IsReadOnly = true };
            var deviceBox = new TextBox { Width = 150, Margin = new Thickness(5), Text = device, IsReadOnly = true };
            var removeButton = new Button { Content = "X", Width = 20, Margin = new Thickness(5) };

            removeButton.Click += (s, e) => SerialStackPanel.Children.Remove(border);

            rowPanel.Children.Add(serialBox);
            rowPanel.Children.Add(inventoryBox);
            rowPanel.Children.Add(deviceBox);
            rowPanel.Children.Add(removeButton);

            border.Child = rowPanel;

            SerialStackPanel.Children.Insert(0, border);
        }

    }
}
