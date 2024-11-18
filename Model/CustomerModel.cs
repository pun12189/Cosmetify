using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Model
{
    public class CustomerModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
        
        public string State { get; set; }
        
        public string District { get; set; }
        
        public int PinCode { get; set; }
        
        public string Country { get; set; }

        public DateTime DateOfAnniversary { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Label {  get; set; }

        public string Notes { get; set; }

    }
}
