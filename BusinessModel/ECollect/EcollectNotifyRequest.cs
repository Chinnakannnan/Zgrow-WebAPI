using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.ECollect
{
    public class EcollectNotifyRequest
    {
    /*    public EcollectNotifyRequest()
            {
            notify = new Notify();
        }*/
        public Notify notify { get; set; }
    }

    public class Notify
    {
        public string customer_code { get; set; }
        public string bene_account_no { get; set; }
        public string bene_account_ifsc { get; set; }
        public string bene_full_name { get; set; }
        public string transfer_type { get; set; }
        public string transfer_unique_no { get; set; }
        public string transfer_timestamp { get; set; }
        public string transfer_ccy { get; set; }
        public string transfer_amt { get; set; }
        public string rmtr_account_no { get; set; }
        public string rmtr_account_ifsc { get; set; }
        public string rmtr_account_type { get; set; }
        public string rmtr_full_name { get; set; }
        public string rmtr_address { get; set; }
        public string rmtr_to_bene_note { get; set; }
        public string attempt_no { get; set; }
        public string status { get; set; }
        public string credit_acct_no { get; set; }
        public string credited_at { get; set; }

    }
}
