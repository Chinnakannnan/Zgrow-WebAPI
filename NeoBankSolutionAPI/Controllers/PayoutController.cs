using BusinessDomain.Payout;
using BusinessModel.Auth;
using BusinessModel.Common;
using BusinessModel.Payout;
using RuleEngine.Payout;
using DataAccess.Payout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace NeoBankSolutionAPI.Controllers
{
    [Authorize]
    [Route("api/payout")]
    [ApiController]
    public class PayoutController : ControllerBase
    {
        private readonly IPayoutBusiness _businessDomain;
        private readonly IConfiguration _iconfiguration;
        private readonly IPayoutRule _payoutRuleEngine;

        public PayoutController(IPayoutBusiness businessInstance, IConfiguration iconfiguration, IPayoutRule ruleInstance) => (_businessDomain, _iconfiguration, _payoutRuleEngine) = (businessInstance, iconfiguration, ruleInstance);      

        [HttpPost]
        [Route("FundTransfer")]
        public IActionResult FundTransfer(PayoutRequest payoutRequest)
        {
            try {
                string CompanyCode = string.Empty;
                if (Request.Headers.TryGetValue("CompanyCode", out var companycode)) 
                { 
                    CompanyCode = companycode; 
                }
                if (string.IsNullOrEmpty(CompanyCode))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Company_Code });
                if (string.IsNullOrEmpty(payoutRequest.Amount) || string.IsNullOrEmpty(payoutRequest.AccountNumber) || string.IsNullOrEmpty(payoutRequest.CustomerId) || string.IsNullOrEmpty(payoutRequest.IfscCode))  
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InputEmpty });  
                if (payoutRequest == null) 
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });
                var validationResult = _businessDomain.PrimaryCheck("FundTransfer", CompanyCode, payoutRequest.CustomerId);
                if (validationResult == null) 
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.UnExpected }) ;
                var actionResult = _payoutRuleEngine.Payout(validationResult, payoutRequest);
                return Ok(actionResult);
                } 
            catch(Exception Ex){ throw; }
           
        }

        [HttpPost]
        [Route("CheckStatus")]
        public IActionResult CheckStatus(PayoutCheckRequest payoutCheckRequest)
        {
            try
            {
                string CompanyCode = string.Empty;
                if (Request.Headers.TryGetValue("CompanyCode", out var companycode))
                {
                    CompanyCode = companycode;
                }                
                var validationResult = _businessDomain.PrimaryCheck("CheckStatus", CompanyCode, payoutCheckRequest.CustomerId);

                if (validationResult == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.UnExpected });
                var actionResult = _payoutRuleEngine.CheckStatus(validationResult, payoutCheckRequest);
                return Ok(actionResult);
            }
            catch (Exception Ex) { throw; }

        }





    }
}
