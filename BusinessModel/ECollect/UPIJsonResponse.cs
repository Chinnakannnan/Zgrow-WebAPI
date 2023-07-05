using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.ECollect
{
    public class UPIJsonResponse
    {
        public UPIJsonResponse()
        {
            validateResponse = new ValidateResponse();
        }
        public ValidateResponse validateResponse { get; set; }
    }

    public class ValidateResponse
    {
        public string decision { get; set; }
    }
}
