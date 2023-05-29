using BusinessModel.Payout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Payout
{
    public interface IPayoutBusiness
    {
        PrimaryCheckModel PrimaryCheck(string APIName, String CompanyCode, string CustomerId);

    }
}