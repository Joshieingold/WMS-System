using Npgsql;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
                    var data = GetDeviceData(serial);
                    if (data.HasValue)
                    {
                        AddSerialRow(serial, data.Value.SubInventory, data.Value.Locator);
                    }
                    else
                    {
                        MessageBox.Show("Serial not found in NB1 inventory.");
                    }

                    SerialInputBox.Clear();
                }
            }
        }

        private (string SubInventory, string Locator)? GetDeviceData(string serial)
        {
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                    SELECT sub_inventory, locator 
                    FROM inventory 
                    WHERE serial = @serial AND organization = 'NB1'", conn);

                cmd.Parameters.AddWithValue("serial", serial);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (
                        reader["sub_inventory"].ToString(),
                        reader["locator"].ToString()
                    );
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void AddSerialRow(string serial, string inventory, string locator)
        {
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
            var locatorBox = new TextBox { Width = 150, Margin = new Thickness(5), Text = locator, IsReadOnly = true };
            var removeButton = new Button { Content = "X", Width = 20, Margin = new Thickness(5) };

            removeButton.Click += (s, e) => SerialStackPanel.Children.Remove(border);

            rowPanel.Children.Add(serialBox);
            rowPanel.Children.Add(inventoryBox);
            rowPanel.Children.Add(locatorBox);
            rowPanel.Children.Add(removeButton);

            border.Child = rowPanel;

            SerialStackPanel.Children.Insert(0, border);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string lpn = LpnInputBox.Text.Trim();
            string newSubInventory = InventoryTextBox.Text.Trim();
            string newLocation = LocationTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(lpn) || string.IsNullOrWhiteSpace(newSubInventory) || string.IsNullOrWhiteSpace(newLocation))
            {
                MessageBox.Show("All fields (LPN, Sub-Inventory, Location) must be filled.");
                return;
            }

            int count = 0;

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            foreach (Border border in SerialStackPanel.Children)
            {
                if (border.Child is StackPanel row &&
                    row.Children[0] is TextBox serialBox)
                {
                    string serial = serialBox.Text;

                    using var cmd = new NpgsqlCommand(@"
                        UPDATE inventory
                        SET sub_inventory = @sub_inventory,
                            locator = @locator,
                            organization = 'NB1',
                            lpn = @lpn
                        WHERE serial = @serial AND organization = 'NB1'", conn);

                    cmd.Parameters.AddWithValue("sub_inventory", newSubInventory);
                    cmd.Parameters.AddWithValue("locator", newLocation);
                    cmd.Parameters.AddWithValue("serial", serial);
                    cmd.Parameters.AddWithValue("lpn", lpn);

                    count += cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show($"{count} serial(s) updated.");
            SerialStackPanel.Children.Clear();
            LpnInputBox.Clear();
            InventoryTextBox.Clear();
            SerialInputBox.Clear();
        }
    }
}
