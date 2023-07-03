using BusinessModel.PaymentGateway;
using BusinessModel.Payout;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PaymentGateway
{
    public class PaymentGatewayDA  : DapperRepository<object>, IPaymentGatewayDA
    {
        public PaymentGatewayDA(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {

        }
        public async Task<bool> TxnInitiate(Dictionary<string, string> dict, string TxnId, InitiateRequest initiateRequest, string result)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@TxnID", TxnId);
                dParam.Add("@Request", dict.ToString());
                dParam.Add("@Response", result.ToString());
                dParam.Add("@CustomerId", initiateRequest.CustomerId);
                dParam.Add("@Amount", initiateRequest.Amount);
                dParam.Add("@MobileNumber", initiateRequest.MobileNumber);
                dParam.Add("@Description", initiateRequest.Description);
                dParam.Add("@Name", initiateRequest.Name);
                dParam.Add("@MailId", initiateRequest.MailId);
                dParam.Add("@IpAddress", initiateRequest.IpAddress);
                dParam.Add("@Latitude", initiateRequest.Latitude);
                dParam.Add("@Longitude", initiateRequest.Longitude);
                var dBresult = await QuerySPTask("sp_InsertPaymentGatewayTxn", dParam);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> Txnupdate(UpdateStatusResponse updateStatusResponse)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@TxnID", updateStatusResponse.txnID);
                dParam.Add("@Request", updateStatusResponse.Request);
                dParam.Add("@Response", updateStatusResponse.Response);
                dParam.Add("@Status", updateStatusResponse.Status);

                var dBresult = await QuerySPTask("sp_updateGatewayTxn", dParam);
                return dBresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public BasicTxnInfo GetTxnBasicInfo(GetStatusRequest getStatusRequest)
        { 
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@TxnID", getStatusRequest.txnID);

                var result = QuerySP<BasicTxnInfo>("sp_GetTxnBasicInfo", dParam).FirstOrDefault();               

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
