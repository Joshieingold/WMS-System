using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Class_Library
{
    public class OrderDevice
    {
        public string sku { get; set; }
        public int qty { get; set; }
        public string requiredInventory { get; set; }
        public string shipmentType { get; set; }
        public OrderDevice(string Sku, int Qty, string RequiredInventory, string ShipmentType)
        {
            sku = Sku;
            qty = Qty;
            requiredInventory = RequiredInventory;
            shipmentType = ShipmentType;
        }
        public void print()
        {
            Console.WriteLine($"Device: {sku} | Quantity {qty} | From inventory: {requiredInventory} | Type: {shipmentType}");
        }
    }
}
