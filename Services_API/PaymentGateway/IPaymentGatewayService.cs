using BusinessModel.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_API.PaymentGateway
{
    public interface IPaymentGatewayService
    {
        LinkResponse EasebuzzPaymentInitiate(InitiateRequest initiateRequest);
        string EasebuzzCheckststatus(GetStatusRequest getStatusRequest);
    }
}
