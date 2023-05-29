using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Payout
{
    public class TransationInsert
    {
        public string CustomerId { get; set; }
        public string ReferenceID { get; set; }
        public string Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionMode { get; set; }
        public string TransactionType { get; set; }
        public string TransactionBank { get; set; }
        public string BeneName { get; set; }
        public string BeneAccountNo { get; set; }
        public string BeneIfsc { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        
    }
}
