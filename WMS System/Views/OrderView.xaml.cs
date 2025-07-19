using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMS_Class_Library;
using WMS_Class_Library.DatabaseClasses;
using WMS_Class_Library.UIClasses;
using WMS_System.Components;

namespace WMS_System.Views
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        public OrderView()
        {
            InitializeComponent();
            List<int> pendingOrders = Database.GetPendingOrders();
            pendingOrderBox.ItemsSource = pendingOrders.Select(id => $"Order #{id}");
            var initialEntry = new SerialEntryUI("Transfer", "4093075082100148");
            SerialStackPanel.Children.Add(initialEntry);
        }
        private void PendingOrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pendingOrderBox.SelectedItem != null)
            {
                // Extract the order ID from the selected item text (e.g., "Order #3")
                string selectedText = pendingOrderBox.SelectedItem.ToString();
                if (int.TryParse(selectedText.Replace("Order #", ""), out int orderId))
                {
                    Order selectedOrder = Database.CreateOrderById(orderId);
                    selectedOrder.print();
                }
            }
        }

    }
}
