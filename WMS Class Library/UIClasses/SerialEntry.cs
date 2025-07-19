using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Class_Library.UIClasses
{
    public class SerialEntry
    {
        public string use { get; set; } // determines what features are active
        public string serial { get; set; }
        public string statusColor { get; set; }  // Blue, Green, Red
        public string subInventory { get; set; }
        public string locator { get; set; }
        public string sku { get; set; }

        public SerialEntry(string Use, string Serial)
        {
            use = Use;
            serial = Serial;
            var details = DatabaseClasses.Database.GetSerialDatabaseDetails(serial);
            locator = details.locator;
            sku = details.sku;
            subInventory = details.subInventory;


            if (use == "Transfer")
            {
                statusColor = "None";
            }
            else
            {
                statusColor = DetermineStatusColor();
            }


        }
        private string DetermineStatusColor()
        {
            return "hi";
        }
    
    
    public void print()
        {
            Console.WriteLine($"{serial} | {sku} | {subInventory} | {locator}");
        }
    }
}