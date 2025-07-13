using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Class_Library
{
    public class OrderDetailDisplay
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }

        public override string ToString() => $"{Sku}: {Quantity}";
    }
}