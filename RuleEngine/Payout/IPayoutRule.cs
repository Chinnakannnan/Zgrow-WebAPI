using BusinessModel.Common;
using BusinessModel.Payout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Payout
{
    public interface IPayoutRule 
    {
        StatusResponse Payout(PrimaryCheckModel primaryCheck, PayoutRequest payoutRequest);
        StatusResponse CheckStatus(PrimaryCheckModel primaryCheck, PayoutCheckRequest payoutCheckRequest);

    }
}
