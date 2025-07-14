using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Class_Library.DatabaseClasses
{
    public class InventoryItem
    {
        public int deviceId { get; set; }
        public string subInventory { get; set; }
        public string org { get; set; }
        public string serial { get; set; }
        public string locator { get; set; }
        public string lpn { get; set; }
    }
}
