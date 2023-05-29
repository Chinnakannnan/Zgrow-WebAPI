using BusinessModel.Common;
using BusinessModel.Payout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Payout
{
    public interface IPayoutDA
    {
        PrimaryCheckModel PrimaryCheck(string APIName, String CompanyCode, string CustomerId);
        UserModel CustomerInfo(string CustomerId);
        async Task<bool> TxnInsert(TransationInsert transationInsert) { try { return true; } catch { return false; } }
        async Task<bool> TxnUpdate(TransationInsert transationInsert) { try { return true; } catch { return false; } }
        async Task<bool> TxnUpdateStatus(TransationInsert transationInsert) { try { return true; } catch { return false; } }
    }
}
