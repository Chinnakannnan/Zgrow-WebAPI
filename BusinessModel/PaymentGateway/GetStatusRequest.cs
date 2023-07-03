using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.PaymentGateway
{
    public class GetStatusRequest
    {        
        public string CustomerId { get; set; }
        public string txnID { get; set; }


    }
}
