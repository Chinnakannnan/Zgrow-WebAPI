using BusinessModel.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.PaymentGateway
{
    public interface IPaymentGatewayBussiness
    {
        Task<bool> TxnUpdate(UpdateStatusResponse updateStatusResponse);
    }
}
