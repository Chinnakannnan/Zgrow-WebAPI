using BusinessModel.PaymentGateway;
using BusinessModel.Payout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PaymentGateway
{
    public interface IPaymentGatewayDA
    {

        async Task<bool> TxnInitiate(Dictionary<string, string> dict,string TxnId, InitiateRequest initiateRequest, string result ) { try { return true; } catch { return false; } }

        BasicTxnInfo GetTxnBasicInfo(GetStatusRequest getStatusRequest);
        async Task<bool> Txnupdate(UpdateStatusResponse updateStatusResponse) { try { return true; } catch { return false; } } 

    }
}
