/*  -----------   Controller OverView Start----------------------   
       
1)Should Use Applog and ErrorLog method for log Entry
         Example: 
         try
         {
         await _LogRepository.AppLog(ControllerName.Admin, MethodName.AddUser, "method input".ToString());
         catch (Exception ex)
         {
          await _LogRepository.ErrorLog(ControllerName.Admin, MethodName.AddUser, ex.ToString());
         }


    -----------   Controller OverView End----------------------   */

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
using BusinessModel.PaymentGateway;
using Newtonsoft.Json;
using DataAccess.Common;

namespace NeoBankSolutionAPI.Controllers
{
    [Authorize]
    [Route("api/payout")]
    [ApiController]
    public class PayoutController : ControllerBase
    {
        string CustomerID = string.Empty;
        string CompanyCode = string.Empty;
        private readonly CommonDA _LogRepository;
        private readonly IPayoutBusiness _businessDomain;
        private readonly IConfiguration _iconfiguration;
        private readonly IPayoutRule _payoutRuleEngine;

        public PayoutController(CommonDA LogInstance,IPayoutBusiness businessInstance, IConfiguration iconfiguration, IPayoutRule ruleInstance) => (_LogRepository,_businessDomain, _iconfiguration, _payoutRuleEngine) = (LogInstance,businessInstance, iconfiguration, ruleInstance);      

        [HttpPost]
        [Route("FundTransfer")]
        public async Task<ActionResult> FundTransfer(PayoutRequest payoutRequest)
        {
            try {

                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });
                 await _LogRepository.AppLog(CustomerID, ControllerName.Payout, MethodName.FundTransfer, JsonConvert.SerializeObject(payoutRequest).ToString());
                                
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
            catch(Exception Ex){
                await _LogRepository.ErrorLog(CustomerID, ControllerName.Payout, MethodName.FundTransfer, Ex.ToString());
                throw;
            }
           
        }
        [HttpPost]
        [Route("CheckStatus")]
        public async Task<IActionResult> CheckStatus(PayoutCheckRequest payoutCheckRequest)
        {
            try
            {
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });
                await _LogRepository.AppLog(CustomerID, ControllerName.Payout, MethodName.CheckStatus, JsonConvert.SerializeObject(payoutCheckRequest).ToString());

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
            catch (Exception Ex) {
                await _LogRepository.ErrorLog(CustomerID, ControllerName.Payout, MethodName.CheckStatus, Ex.ToString());

                throw; 
            }

        }





    }
}
