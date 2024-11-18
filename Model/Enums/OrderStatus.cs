using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model.Enums
{
    public enum OrderStatus
    {
        Accepted = 0, 
        
        Awaiting = 1,

        Cancelled = 2,

        Shipped = 3,

        Dispatched = 4,

        Completed = 5,
    }
}
