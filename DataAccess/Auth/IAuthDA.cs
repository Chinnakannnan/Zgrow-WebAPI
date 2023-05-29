using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Auth;
using BusinessModel.Common;

namespace DataAccess.Auth
{
    public interface IAuthDA
    {
        StatusResponse TokenUpdate(string Username, String Token, String RefreshToken);
        StatusResponse Authenticate(Users users, string clientId = null, string clientSecret = null);
        Tokens GetRefreshtoken(string Username, String RefreshToken);
    }
}