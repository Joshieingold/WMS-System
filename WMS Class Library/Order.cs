using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Class_Library
{
    public class Order
    {
        public int OrderId { get; set; }
        public string FromOrg { get; set; }
        public string Status { get; set; }

        public string DisplayOrder => $"Order #{OrderId}";
    }

}
