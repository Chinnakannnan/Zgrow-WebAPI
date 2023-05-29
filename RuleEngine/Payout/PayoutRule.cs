using BusinessModel.Common;
using BusinessModel.Payout;
using Newtonsoft.Json;
using Services_API.Banking;

namespace RuleEngine.Payout
{
    public class PayoutRule : IPayoutRule
    {
     
        private readonly IBankService _bankservice;
        public PayoutRule(IBankService bankconfiguration) => (_bankservice) = (bankconfiguration);       
        public StatusResponse Payout(PrimaryCheckModel primaryCheck, PayoutRequest payoutRequest)
        {

            if (primaryCheck.statuscode == ResponseCode.Success)
            {
                if (primaryCheck.PreferenceAction == CommonConstants.One) 
                {
                    var payoutResponse = _bankservice.YesBankPayout(payoutRequest);
                    
                    if (payoutResponse != null) {
                        YesBankResponse responseStatus = JsonConvert.DeserializeObject<YesBankResponse>(payoutResponse);
                        if (responseStatus.Data.Status == "Received") {

                            // divert to status check

                            return new StatusResponse { StatusCode = "", StatusDesc ="", Data = "" };
                        }
                        else
                        return new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Failed, Data = payoutResponse };
                     }
                    
                }

                if (primaryCheck.PreferenceAction == CommonConstants.Two) // BANK 2
                {
                    //Write Action
                   return new StatusResponse { StatusCode = "", StatusDesc = "" };
                }
            }
            else
            {
                return new StatusResponse { StatusCode = primaryCheck.statuscode, StatusDesc = primaryCheck.statusdesc };
            }

            return new StatusResponse();
        }

        public StatusResponse CheckStatus(PrimaryCheckModel primaryCheck, PayoutCheckRequest payoutCheckRequest)
        {

            if (primaryCheck.statuscode == ResponseCode.Success)
            {
                if (primaryCheck.PreferenceAction == CommonConstants.Four)
                {
                    var payoutStatusResponse = _bankservice.YesBankPaymentStatus(payoutCheckRequest);

                    if (payoutStatusResponse != null)
                    {
                        YesBankResponse responseStatus = JsonConvert.DeserializeObject<YesBankResponse>(payoutStatusResponse);
                        if (responseStatus.Data.Status == "FAILED")
                        {

                           

                            return new StatusResponse { StatusCode = "", StatusDesc = "", Data = "" };
                        }
                        else
                            return new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Failed, Data = payoutStatusResponse };
                    }

                }

                if (primaryCheck.PreferenceAction == CommonConstants.Two) // BANK 2
                {
                    //Write Action
                    return new StatusResponse { StatusCode = "", StatusDesc = "" };
                }
            }
            else
            {
                return new StatusResponse { StatusCode = primaryCheck.statuscode, StatusDesc = primaryCheck.statusdesc };
            }

            return new StatusResponse();
        }


    }
}
