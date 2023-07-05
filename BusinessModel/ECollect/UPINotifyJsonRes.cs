using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.ECollect
{     
    public class UPINotifyJsonRes
    {
        public UPINotifyJsonRes()
        {
            notifyResult = new NotifyResponse();
        }
        public NotifyResponse notifyResult { get; set; }
    }

    public class NotifyResponse
    {
        public string result { get; set; }
    }
}
