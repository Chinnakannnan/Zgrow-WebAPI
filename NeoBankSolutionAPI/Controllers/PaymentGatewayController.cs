using BusinessDomain.Payout;
using BusinessModel.Common;
using BusinessModel.PaymentGateway;
using BusinessModel.Payout;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RuleEngine.PaymentGateway;
using RuleEngine.Payout;
using System.ComponentModel.DataAnnotations;
using XAct.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace NeoBankSolutionAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {


        private readonly IPayoutBusiness _businessDomain;
        private readonly IConfiguration _iconfiguration;
        private readonly IPaymentGatwayRule _paymentGatewayRuleEngine;
        public PaymentGatewayController(IPayoutBusiness businessInstance, IConfiguration iconfiguration, IPaymentGatwayRule ruleInstance) => (_businessDomain, _iconfiguration, _paymentGatewayRuleEngine) = (businessInstance, iconfiguration, ruleInstance);
         
        [HttpPost]
        [Route("InitiatePay")]

        public IActionResult InitiatePay(InitiateRequest initiateRequest)
        {
            try
            {
                string CompanyCode = string.Empty;
                if (Request.Headers.TryGetValue("CompanyCode", out var companycode))
                {
                    CompanyCode = companycode;
                }
                if (string.IsNullOrEmpty(CompanyCode))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Company_Code });
                if (string.IsNullOrEmpty(initiateRequest.Amount) || string.IsNullOrEmpty(initiateRequest.Description) || string.IsNullOrEmpty(initiateRequest.CustomerId) || string.IsNullOrEmpty(initiateRequest.MailId) || string.IsNullOrEmpty(initiateRequest.MobileNumber) || string.IsNullOrEmpty(initiateRequest.Name))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InputEmpty });
                if (initiateRequest == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });

                var validationResult = _businessDomain.PrimaryCheck("InitiatePay", CompanyCode, initiateRequest.CustomerId);
                if (validationResult == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.UnExpected });


                var actionResult = _paymentGatewayRuleEngine.InitiatePayment(validationResult, initiateRequest);
              
                
                return Ok(actionResult);
            }
            catch (Exception Ex) { throw; }

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("InitiatePayExternal")]

        public IActionResult InitiatePayExternal(InitiateRequest initiateRequest)
        {
            try
            {                
                if (string.IsNullOrEmpty(initiateRequest.CompanyCode))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Company_Code });
                if (string.IsNullOrEmpty(initiateRequest.Amount) || string.IsNullOrEmpty(initiateRequest.Description) || string.IsNullOrEmpty(initiateRequest.CustomerId) || string.IsNullOrEmpty(initiateRequest.MailId) || string.IsNullOrEmpty(initiateRequest.MobileNumber) || string.IsNullOrEmpty(initiateRequest.Name))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InputEmpty });
                if (initiateRequest == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });

                var validationResult = _businessDomain.PrimaryCheck("InitiatePay", initiateRequest.CompanyCode, initiateRequest.CustomerId);
                if (validationResult == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.UnExpected });


                var actionResult = _paymentGatewayRuleEngine.InitiatePayment(validationResult, initiateRequest);


                return Ok(actionResult);
            }
            catch (Exception Ex) { throw; }

        }


     /*   [HttpPost("InitiateExternalPay")]       
        public IActionResult InitiateExternalPay(InitiateRequestExternal initiateRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(initiateRequest.CompanyCode))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Company_Code });
                if (string.IsNullOrEmpty(initiateRequest.Amount) || string.IsNullOrEmpty(initiateRequest.Description) || string.IsNullOrEmpty(initiateRequest.CustomerId) || string.IsNullOrEmpty(initiateRequest.MailId) || string.IsNullOrEmpty(initiateRequest.MobileNumber) || string.IsNullOrEmpty(initiateRequest.Name))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InputEmpty });
                if (initiateRequest == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });

                var validationResult = _businessDomain.PrimaryCheck("InitiatePay", initiateRequest.CompanyCode, initiateRequest.CustomerId);
                if (validationResult == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.UnExpected });
                if (validationResult.statuscode !="000")
                    return BadRequest(new StatusResponse { StatusCode = validationResult.statuscode, StatusDesc = validationResult.statusdesc});

                InitiateRequest value = new InitiateRequest();
                value.CustomerId= initiateRequest.CustomerId;
                value.Name = initiateRequest.Name;
                value.MobileNumber = initiateRequest.MobileNumber;
                value.MailId=initiateRequest.MailId;
                value.Amount = initiateRequest.Amount;
                value.Description = initiateRequest.Description;

                StatusResponse actionResult = _paymentGatewayRuleEngine.InitiatePaymentwol(validationResult, value);

                var link = actionResult.Data.ToString();
                return Redirect(link);

            }
            catch (Exception Ex) { throw; }

        }       
*/

        [HttpPost]
        [Route("GetPayStatus")]
        public IActionResult GetPayStatus(GetStatusRequest getStatusRequest)
        {
            try
            {
                string CompanyCode = string.Empty;
                if (Request.Headers.TryGetValue("CompanyCode", out var companycode))
                {
                    CompanyCode = companycode;
                }
                if (string.IsNullOrEmpty(CompanyCode))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Company_Code });
                if (getStatusRequest == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });
                if ( string.IsNullOrEmpty(getStatusRequest.txnID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InputEmpty });

                var validationResult = _businessDomain.PrimaryCheck("GetPayStatus", CompanyCode, getStatusRequest.CustomerId);
                if (validationResult == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.UnExpected });

                 
                var actionResult = _paymentGatewayRuleEngine.Checkstatus(validationResult, getStatusRequest);

                 
                return Ok(actionResult);
            }
            catch (Exception Ex) { throw; }



        }

    }
}
