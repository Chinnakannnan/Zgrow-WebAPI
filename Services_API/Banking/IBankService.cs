using BusinessModel.Payout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_API.Banking
{
    public interface IBankService
    {
        string YesBankPayout(PayoutRequest payoutRequest);
        string AxisBankPayout(PayoutRequest payoutRequest);
        string YesBankPaymentStatus(PayoutCheckRequest payoutCheckRequest);
    }
}
