using Cosmetify.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string OrderId { get; set;}

        public PaymentType PaymentType { get; set; }

        public double Amount { get; set; }

        public double Balance { get; set; }

        public CustomerModel Customer { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string TakenBy { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public ObservableCollection<OrderedProduct> OrderedProducts { get; set; }

        public DateTime NextFollowup { get; set; }

        public double AdvanceAmount { get; set; }

        public DateTime BillDate { get; set; }

        public string BillNumber { get; set; }

        public bool Priority { get; set; }

        public double Discount { get; set; }

        public double AmountAfterDiscount { get; set; }
    }
}
