using BusinessModel.Admin;
using BusinessModel.Common;
using BusinessModel.PaymentGateway;
using DataAccess.Admin;
using DataAccess.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Admin
{
    public class AdminBusiness : IAdminBusiness
    {
        private readonly IAdminDA _adminRepository; 
        public AdminBusiness(IAdminDA AdminRepositoryInstance) => (_adminRepository) = (AdminRepositoryInstance);

        public StatusResponse AddCompany(AddAdminRequest addAdminRequest)       
        {
            var result = _adminRepository.AddCompany(addAdminRequest);
            return result;
        }
        public StatusResponse AddUser(OnBoardingRequest onBoardingRequest)
        {
            StatusResponse result = _adminRepository.AddUser(onBoardingRequest);
            return result;
        }
     
        public StatusResponse APIInsert(APIInsert apiRequest)
        {
            StatusResponse result = _adminRepository.AddAPI(apiRequest);
            return result;
        }

    }
}
