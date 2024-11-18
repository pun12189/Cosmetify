using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitaab.Model
{
    public class TransactionModel
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionId { get; set; }

        public string Message { get; set; }

        public int Order_Id { get; set; }
    }
}
