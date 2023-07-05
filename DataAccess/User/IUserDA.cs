using BusinessModel.Admin;
using BusinessModel.Payout;
using BusinessModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.User
{
    public interface IUserDA
    {
        List<CompanyList> GetCompanyList();
        UserInfoResponse UserInfo(UserInfo userInfo);
        async Task<bool> ErrorLog(string Content) { try { return true; } catch { return false; } }
        async Task<bool> AppLog(string Content) { try { return true; } catch { return false; } }

    }
}
