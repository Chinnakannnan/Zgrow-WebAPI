using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Common
{
 
    public class StatusResponse
    {
        public string StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public object Data { get; set; }

    }
}
