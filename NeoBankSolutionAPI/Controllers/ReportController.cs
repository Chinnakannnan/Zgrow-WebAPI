using BusinessDomain.PaymentGateway;
using BusinessDomain.Payout;
using BusinessDomain.Report;
using BusinessModel.Common;
using BusinessModel.PaymentGateway;
using BusinessModel.Payout;
using BusinessModel.Report;
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
        private readonly IReportBusiness _businessDomain;
        private readonly IPaymentGatewayService _paymentGatewayDomain;
        private readonly IPaymentGatewayBussiness _paymentGatewayBusiness;
        

        public ReportController(IReportBusiness businessInstance, IPaymentGatewayService GatewayBuinessInstance, IPaymentGatewayBussiness PaymentGatewayBusinessInstance) => (_businessDomain, _paymentGatewayDomain, _paymentGatewayBusiness) = (businessInstance, GatewayBuinessInstance, PaymentGatewayBusinessInstance);

        [HttpPost]
        [Route("PaymentGatewayReport")]
        public IActionResult PaymentGatewayReport(Reportrequest report)
        {
            try
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
                
                return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = ResponseMessage.Success,Data= actionResult });
            }
            catch (Exception Ex) { throw; }

        }

    }
}
