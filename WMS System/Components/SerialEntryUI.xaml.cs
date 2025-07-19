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
using WMS_Class_Library.UIClasses;

namespace WMS_System.Components
{
    /// <summary>
    /// Interaction logic for SerialEntryUI.xaml
    /// </summary>
    public partial class SerialEntryUI : UserControl
    {
        private SerialEntry entry;

        public SerialEntryUI(string use, string serial)
        {
            InitializeComponent();

            entry = new SerialEntry(use, serial);
            PopulateFields();
        }

        private void PopulateFields()
        {
            SerialTextBox.Text = entry.serial;
            DeviceNameTextBox.Text = entry.sku;
            LocatorTextBox.Text = entry.locator;

            // Optionally set background color based on statusColor
            switch (entry.statusColor)
            {
                case "Green":
                    Background = Brushes.LightGreen;
                    break;
                case "Red":
                    Background = Brushes.IndianRed;
                    break;
                case "Blue":
                    Background = Brushes.LightBlue;
                    break;
                default:
                    Background = Brushes.Transparent;
                    break;
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as Panel;
            parent?.Children.Remove(this);
        }

    }


}
