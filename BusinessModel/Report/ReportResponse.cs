using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Report
{
    public class ReportResponse
    { 
        public string CustomerId { get; set; }
        public string Reference_Id  { get; set; }
        public string BeneMobileNumber  { get; set; }
        public string Amount { get; set; }
        public string MailId { get; set; }
        public string TransactionStatus { get; set; } 
        public string CreatedTime { get; set; }
    }

}
