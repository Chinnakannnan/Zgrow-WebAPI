using BusinessModel.Common;
using BusinessModel.PaymentGateway;
using BusinessModel.Payout;

namespace RuleEngine.PaymentGateway
{
    public interface IPaymentGatwayRule
    {
        StatusResponse InitiatePayment(PrimaryCheckModel primaryCheck, InitiateRequest initiateRequest);

        StatusResponse Checkstatus(PrimaryCheckModel primaryCheck, GetStatusRequest getStatusRequest);
        StatusResponse InitiatePaymentwol(PrimaryCheckModel primaryCheck, InitiateRequest initiateRequest);
    }
}
