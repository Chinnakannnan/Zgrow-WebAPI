using BusinessModel.Admin;
using BusinessModel.User;
using DataAccess.Payout;
using DataAccess.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.User
{
    public class UserBusiness: IUserBusiness
    {
        private readonly IUserDA _userRepository; 
        public UserBusiness(IUserDA authRepositoryInstance) => (_userRepository) = (authRepositoryInstance);
        public List<CompanyList> GetCompanyList()
        {
            List<CompanyList> result = _userRepository.GetCompanyList();
            return result;
        }
        public UserInfoResponse UserInfo(UserInfo userInfo)
        {
           var result= _userRepository.UserInfo(userInfo);
            return result;

        }

    }
}
