using BusinessModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.User
{
    public interface IUserBusiness
    {
        UserInfoResponse UserInfo(UserInfo userInfo);


    }
}
