using BusinessModel.PaymentGateway;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using BusinessModel.ECollect;
using BusinessModel.Common;
using DataAccess.Common;


namespace NeoBankSolutionAPI.Controllers
{
    [Route("ecollect")]
    [ApiController]
    public class ECollectController : ControllerBase
    {
        private readonly CommonDA _Repository; 
        public ECollectController(CommonDA commonInstance) => (_Repository) = (commonInstance);

        [HttpPost]
        [Route("notify")]  
        public async Task<IActionResult> Notify(object Request)
        {
           
            UPINotifyJsonRes valResponse = new UPINotifyJsonRes();
            string lstrReturn = string.Empty;  
            EcollectNotifyRequest eCol = new EcollectNotifyRequest();
            try
            {  
                if (Request == null)
                {
                    valResponse.notifyResult.result = "Bad Request";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return BadRequest(lstrReturn);
                }

               await _Repository.AppLog("ECollect", "Notify", Request.ToString());           

                //  objLog.WriteAppLog("Ecoll Notify : data :" + lstrRequest, lstrFolderName);

                EcollectNotifyRequest eColReq = JsonConvert.DeserializeObject<EcollectNotifyRequest>(Request.ToString());

                if (eColReq.notify.bene_account_no == YesBankEcollection.OkAccountNumber && eColReq.notify.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.notify.transfer_type == YesBankEcollection.Imps)
                {
                    valResponse.notifyResult.result = "ok";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return Ok(lstrReturn);
                }
                else if (eColReq.notify.bene_account_no == YesBankEcollection.RetryAccountNumber && eColReq.notify.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.notify.transfer_type == YesBankEcollection.Imps)
                {
                    valResponse.notifyResult.result = "Retry";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return Ok(lstrReturn);
                }
                else if (eColReq.notify.bene_account_no == YesBankEcollection.ISEAccountNumber && eColReq.notify.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.notify.transfer_type == YesBankEcollection.Neft)
                {
                    valResponse.notifyResult.result = "Internal Server Error";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return StatusCode(500, lstrReturn);
                }
                else if (eColReq.notify.bene_account_no == YesBankEcollection.UnAuthorisedAccountNumber && eColReq.notify.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.notify.transfer_type == YesBankEcollection.Rtgs)
                {
                    valResponse.notifyResult.result = "UnAuthorised";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return Unauthorized(lstrReturn);
                }
                else if (eColReq.notify.bene_account_no == YesBankEcollection.BadRequestAccountNumber && eColReq.notify.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.notify.transfer_type == YesBankEcollection.Rtgs)
                {
                    valResponse.notifyResult.result = "Bad Request";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return BadRequest(lstrReturn);
                }
                else
                {
                    valResponse.notifyResult.result = "rejected! -  BAD REQUEST ";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return BadRequest(lstrReturn);
                }

            }
            catch (Exception ex)
            {

                 await _Repository.ErrorLog("ECollect", "Notify", ex.ToString());
                valResponse.notifyResult.result = "Internal Server Error";
                lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                return StatusCode(500, lstrReturn);
            }



        }

        [HttpPost]
        [Route("validateuser")]
        public async Task<IActionResult> Validate(object Request)      
        {
 
            UPIJsonResponse valResponse = new UPIJsonResponse();
            string lstrReturn = string.Empty;
            HttpResponseMessage Response = new HttpResponseMessage();
            EcollectRequest eCol = new EcollectRequest();
            try
            {
                string lstrRequest = string.Empty;
                if (Request == null)
                {
                    valResponse.validateResponse.decision = "Bad Request";

                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return BadRequest(lstrReturn);
                }
                await _Repository.AppLog("ECollect", "Validate", Request.ToString()); 

                EcollectRequest eColReq = JsonConvert.DeserializeObject<EcollectRequest>(Request.ToString());


                if (eColReq.validate.bene_account_no == YesBankEcollection.OkAccountNumber && eColReq.validate.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.validate.transfer_type == YesBankEcollection.Imps)
                {
                    valResponse.validateResponse.decision = "pass";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return Ok(lstrReturn);
                }

                else if (eColReq.validate.bene_account_no == YesBankEcollection.RejectAccountNumber && eColReq.validate.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.validate.transfer_type == YesBankEcollection.Imps)
                {
                    valResponse.validateResponse.decision = "reject";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return Ok(lstrReturn);
                }
                else if (eColReq.validate.bene_account_no == YesBankEcollection.PendingAccountNumber && eColReq.validate.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.validate.transfer_type == YesBankEcollection.Imps)
                {
                    valResponse.validateResponse.decision = "pending";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return Ok(lstrReturn);
                }
                else if (eColReq.validate.bene_account_no == YesBankEcollection.ISEAccountNumber && eColReq.validate.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.validate.transfer_type == YesBankEcollection.Neft)
                {
                    valResponse.validateResponse.decision = "Internal Server Error";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return StatusCode(500, lstrReturn);
                }
                else if (eColReq.validate.bene_account_no == YesBankEcollection.UnAuthorisedAccountNumber && eColReq.validate.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.validate.transfer_type == YesBankEcollection.Rtgs)
                {
                    valResponse.validateResponse.decision = "UnAuthorised";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);

                    Response.StatusCode = HttpStatusCode.Unauthorized;
                    Response.Content = new StringContent(lstrReturn, System.Text.Encoding.UTF8, "application/json");
                    return Unauthorized(lstrReturn);
                }
                 else if (eColReq.validate.bene_account_no == YesBankEcollection.BadRequestAccountNumber && eColReq.validate.bene_account_ifsc == YesBankEcollection.IFSC && eColReq.validate.transfer_type == YesBankEcollection.Rtgs)
                {
                    valResponse.validateResponse.decision = "Bad Request";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return BadRequest(lstrReturn);
                }
                else
                {
                    valResponse.validateResponse.decision = "rejected! - BAD REQUEST ";
                    lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                    return BadRequest(lstrReturn);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLog("ECollect", "Notify", ex.ToString());
                valResponse.validateResponse.decision = "Internal Server Error";
                lstrReturn = Newtonsoft.Json.JsonConvert.SerializeObject(valResponse);
                return StatusCode(500, lstrReturn);
            }
        }

    }
}
