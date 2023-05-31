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
        private readonly IUserDA _authRepository; 
        public UserBusiness(IUserDA authRepositoryInstance) => (_authRepository) = (authRepositoryInstance);

        public UserInfoResponse UserInfo(UserInfo userInfo)
        {
           var result= _authRepository.UserInfo(userInfo);
            return result;

        }

    }
}
