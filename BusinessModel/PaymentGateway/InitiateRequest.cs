using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.PaymentGateway
{
    public class InitiateRequest
    {
        public string CompanyCode { get; set; }
        public string CustomerId { get; set; }
        public string Amount { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string MailId { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
  

}
