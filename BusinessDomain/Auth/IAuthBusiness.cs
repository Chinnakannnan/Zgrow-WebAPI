
using BusinessModel.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Auth
{
    public interface IAuthBusiness
    {
        Tokens GenerateToken(string userName);
        Tokens GenerateRefreshToken(string userName);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Tokens GetSavedRefreshTokens(String UserName, String RefreshToken);
    

    }
}
