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
using BusinessDomain.PaymentGateway;
using BusinessDomain.Payout;
using BusinessDomain.Report;
using BusinessModel.Common;
using BusinessModel.PaymentGateway;
using BusinessModel.Payout;
using BusinessModel.Report;
using DataAccess.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RuleEngine.Payout;
using Services_API.PaymentGateway;

namespace NeoBankSolutionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        string CustomerID = string.Empty;
        private readonly CommonDA _LogRepository;
        private readonly IReportBusiness _businessDomain;
        private readonly IPaymentGatewayService _paymentGatewayDomain;
        private readonly IPaymentGatewayBussiness _paymentGatewayBusiness;
        

        public ReportController(CommonDA logInstance, IReportBusiness businessInstance, IPaymentGatewayService GatewayBuinessInstance, IPaymentGatewayBussiness PaymentGatewayBusinessInstance) => (_LogRepository,_businessDomain, _paymentGatewayDomain, _paymentGatewayBusiness) = (logInstance,businessInstance, GatewayBuinessInstance, PaymentGatewayBusinessInstance);

        [HttpPost]
        [Route("PaymentGatewayReport")]
        public async Task<IActionResult> PaymentGatewayReport(Reportrequest report)
        {
            try
            {
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });
                await _LogRepository.AppLog(CustomerID, ControllerName.Report, MethodName.PaymentGatewayReport, JsonConvert.SerializeObject(report).ToString());
                {
                    if (report == null)
                        return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });

                    if (string.IsNullOrEmpty(report.CustomerId) || string.IsNullOrEmpty(report.FromDate.ToString()) || string.IsNullOrEmpty(report.ToDate.ToString()))
                        return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InputEmpty });

                    List<ReportResponse> InitiatedId = _businessDomain.InitiatedId();

                    foreach (ReportResponse reportResponse in InitiatedId)
                    {
                        GetStatusRequest getStatusRequest = new GetStatusRequest();
                        getStatusRequest.txnID = reportResponse.Reference_Id;
                        string payoutGatewayResponse = _paymentGatewayDomain.EasebuzzCheckststatus(getStatusRequest);
                        EasebuzzStatusResponse root = JsonConvert.DeserializeObject<EasebuzzStatusResponse>(payoutGatewayResponse);

                        UpdateStatusResponse values = new UpdateStatusResponse();
                        values.txnID = getStatusRequest.txnID;
                        values.CustomerId = getStatusRequest.CustomerId;
                        values.Status = root.msg.status;
                        values.Response = JsonConvert.SerializeObject(root.msg);
                        values.Request = "";

                        _paymentGatewayBusiness.TxnUpdate(values);

                    }
                    List<ReportResponse> actionResult = _businessDomain.PaymentGateWayReport(report);

                    return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = ResponseMessage.Success, Data = actionResult });
                }
            }
            catch (Exception Ex) {
                await _LogRepository.ErrorLog(CustomerID, ControllerName.Report, MethodName.PaymentGatewayReport, Ex.ToString());
                throw;
            }

        }

    }
}
