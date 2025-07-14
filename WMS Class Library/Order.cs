using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Class_Library
{
    public class Order
    {
        public string fromOrg { get; set; }
        public string toOrg { get; set; }
        public string status { get; set; }
        public int orderId { get; set; }
        public string shipmentType { get; set; }
        public List<OrderDevice> requiredDevices { get; set; }
        public Order(string FromOrg, string ToOrg, string Status, int OrderId, List<OrderDevice> RequiredDevices) 
        {
            fromOrg = FromOrg;
            toOrg = ToOrg;
            status = Status;
            orderId = OrderId;
            requiredDevices = RequiredDevices;
        }
        public void print()
        {
            Console.WriteLine($"The following data was pulled from Order #{orderId}");
            Console.WriteLine($"This shipment is going from: {fromOrg}");
            Console.WriteLine($"This shipment is going to: {toOrg}");
            Console.WriteLine($"The status of this order is: {status}");
            foreach (var device in requiredDevices)
            {
                device.print();
            }
        }


    }

}
