using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.PaymentGateway
{
    public class LinkResponse
    {
        public string TxnID { get; set; }
        public object data { get; set; }

    }
    public class LinkResponseFinal
    {
        public string TxnID { get; set; }
        public string Link { get; set; }

    }
    public class ApiResponse
    {
        public string StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public object Data { get; set; }

    }

}
