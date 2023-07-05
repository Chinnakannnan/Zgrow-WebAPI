using BusinessModel.Admin;
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
        List<CompanyList> GetCompanyList();
        UserInfoResponse UserInfo(UserInfo userInfo);


    }
}
