using BusinessModel.PaymentGateway;
using BusinessModel.Payout;
using DataAccess.PaymentGateway;
using DataAccess.Payout;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.PaymentGateway
{
    public class PaymentGatewayBussiness: IPaymentGatewayBussiness
    {

        private readonly IPaymentGatewayDA _authRepository;
        private readonly IConfiguration _iconfiguration;
        public PaymentGatewayBussiness(IPaymentGatewayDA authRepositoryInstance, IConfiguration iconfiguration) => (_authRepository, _iconfiguration) = (authRepositoryInstance, iconfiguration);



        public Task<bool> TxnUpdate(UpdateStatusResponse updateStatusResponse)
        {
            var result = _authRepository.Txnupdate(updateStatusResponse);

            return result;
        }



    }
}
