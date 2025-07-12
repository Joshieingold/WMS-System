using Npgsql;
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

namespace WMS_System.Views
{
    /// <summary>
    /// Interaction logic for TransferView.xaml
    /// </summary>
    public partial class TransferView : UserControl
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=Koreanishard2424$$;Database=postgres";

        public TransferView()
        {
            InitializeComponent();
        }

        private void SerialInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string serial = SerialInputBox.Text.Trim();
                if (!string.IsNullOrWhiteSpace(serial))
                {
                    string inventory = GetInventoryFromDatabase(serial);
                    AddSerialRow(serial, inventory);
                    SerialInputBox.Clear();
                }
            }
        }

        private string GetInventoryFromDatabase(string serial)
        {
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand("SELECT sub_inventory FROM inventory WHERE serial = @serial", conn);
                cmd.Parameters.AddWithValue("serial", serial);

                var result = cmd.ExecuteScalar();
                return result?.ToString() ?? "Not Found";
            }
            catch
            {
                return "DB Error";
            }
        }

        private void AddSerialRow(string serial, string inventory)
        {
            // Create a Border with purple rounded background
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(58, 57, 82)), // Purple
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(2),
                Margin = new Thickness(0, 5, 0, 5)
            };

            var rowPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var serialBox = new TextBox { Width = 200, Margin = new Thickness(5), Text = serial, IsReadOnly = true };
            var inventoryBox = new TextBox { Width = 150, Margin = new Thickness(5), Text = inventory, IsReadOnly = true };
            var removeButton = new Button { Content = "X", Width = 20, Margin = new Thickness(5) };

            removeButton.Click += (s, e) => SerialStackPanel.Children.Remove(border);

            rowPanel.Children.Add(serialBox);
            rowPanel.Children.Add(inventoryBox);
            rowPanel.Children.Add(removeButton);

            border.Child = rowPanel;

            // Insert at the top instead of adding to the end
            SerialStackPanel.Children.Insert(0, border);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            foreach (StackPanel row in SerialStackPanel.Children)
            {
                if (row.Children[0] is TextBox serialBox)
                {
                    string serial = serialBox.Text;
                    string locator = "FLOOR";  // Example hardcoded
                    string organization = "NB1"; // Example hardcoded
                    int deviceId = 1234; // TODO: Let user select device ID

                    using var cmd = new NpgsqlCommand(@"
                        INSERT INTO inventory (device_id, sub_inventory, serial, locator, organization)
                        VALUES (@device_id, @sub_inventory, @serial, @locator, @organization)
                        ON CONFLICT (serial) DO NOTHING", conn);

                    cmd.Parameters.AddWithValue("device_id", deviceId);
                    cmd.Parameters.AddWithValue("sub_inventory", "TRIAGE");
                    cmd.Parameters.AddWithValue("serial", serial);
                    cmd.Parameters.AddWithValue("locator", locator);
                    cmd.Parameters.AddWithValue("organization", organization);

                    count += cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show($"{count} serial(s) transferred.");
            SerialStackPanel.Children.Clear();
        }
    }
}