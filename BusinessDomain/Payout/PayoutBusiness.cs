using BusinessModel.Auth;
using BusinessModel.Payout;
using DataAccess.Payout;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Payout
{
    public class PayoutBusiness : IPayoutBusiness
    {
        private readonly IPayoutDA _authRepository;
        private readonly IConfiguration _iconfiguration;
        public PayoutBusiness(IPayoutDA authRepositoryInstance, IConfiguration iconfiguration) => (_authRepository, _iconfiguration) =(authRepositoryInstance, iconfiguration);
        
        public PrimaryCheckModel PrimaryCheck(string APIName, String CompanyCode, string CustomerId)
        {
            var result = _authRepository.PrimaryCheck(APIName, CompanyCode, CustomerId);
            return result;
        }
        }
}
