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
using BusinessModel.Admin;
using DataAccess.Common;

namespace NeoBankSolutionAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {

        string CompanyCode = string.Empty;
        string CustomerID = string.Empty;
        private readonly CommonDA _LogRepository;
        private readonly IPayoutBusiness _businessDomain;
        private readonly IConfiguration _iconfiguration;
        private readonly IPaymentGatwayRule _paymentGatewayRuleEngine;

        public PaymentGatewayController(CommonDA LogInstance,IPayoutBusiness businessInstance, IConfiguration iconfiguration, IPaymentGatwayRule ruleInstance) => (_LogRepository,_businessDomain, _iconfiguration, _paymentGatewayRuleEngine) = (LogInstance, businessInstance, iconfiguration, ruleInstance);
         
        [HttpPost]
        [Route("InitiatePay")]
        public async Task<ActionResult> InitiatePay(InitiateRequest initiateRequest)
        {
            try
            {             

                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });

                await _LogRepository.AppLog(CustomerID, ControllerName.PaymentGateWay, MethodName.InitiatePay, JsonConvert.SerializeObject(initiateRequest).ToString());


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
            catch (Exception ex) 
            {
                await _LogRepository.ErrorLog(CustomerID, ControllerName.PaymentGateWay, MethodName.InitiatePay, ex.ToString());
                throw; 
            }

        }
       
        [AllowAnonymous]
        [HttpPost]
        [Route("InitiatePayExternal")]
        public async Task<ActionResult> InitiatePayExternal(InitiateRequest initiateRequest)
        {
            try
            {
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });

                await _LogRepository.AppLog(CustomerID, ControllerName.PaymentGateWay, MethodName.InitiatePayExternal, JsonConvert.SerializeObject(initiateRequest).ToString());


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
            catch (Exception Ex) 
            {
                await _LogRepository.ErrorLog(CustomerID, ControllerName.PaymentGateWay, MethodName.InitiatePay, Ex.ToString());
                throw; 
            }

        }
    
        [HttpPost]
        [Route("GetPayStatus")]
        public async Task<ActionResult> GetPayStatus(GetStatusRequest getStatusRequest)
        {
            try
            {
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });
                await _LogRepository.AppLog(CustomerID, ControllerName.PaymentGateWay, MethodName.GetPayStatus, JsonConvert.SerializeObject(getStatusRequest).ToString());
                 
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
            catch (Exception Ex) {

                await _LogRepository.ErrorLog(CustomerID, ControllerName.PaymentGateWay, MethodName.GetPayStatus, Ex.ToString());
                throw;
            }



        }

    }
}
