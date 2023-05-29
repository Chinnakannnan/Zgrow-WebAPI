using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Payout
{
    public class PayoutRequest
    {
        public string CustomerId { get; set; }
        public string Amount { get; set; }
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public string Mode { get; set; }
        public string BeneName { get; set; }
        public string BankName { get; set; }
    }

    public class PayoutCheckRequest
    {
        public string CustomerId { get; set; }
        public string ReferenceId { get; set; }
      
    }




}
