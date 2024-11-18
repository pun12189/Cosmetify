using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitaab.Model
{
    public class OrderedProduct
    {
        public ProductModel Product { get; set; }

        public int Quantity { get; set; }

        public double SubTotal { get; set; }

        public double Total { get; set; }

        public double Gst { get; set; }
    }
}
