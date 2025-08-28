using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class CustomOrderModel
    {
        public string OrderId { get; set; }

        public CustomerModel CustomerName { get; set; }

        public string BrandName { get; set; }

        public int Counter { get; set; }
    }
}
