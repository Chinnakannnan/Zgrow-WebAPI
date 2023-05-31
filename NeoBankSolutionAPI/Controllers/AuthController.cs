using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using BusinessDomain;
using BusinessModel;
using BusinessModel.Auth;
using BusinessDomain.Auth;
using DataAccess.Auth;
using BusinessModel.Common;
using System.IdentityModel.Tokens.Jwt;

namespace NeoBankSolutionAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         private readonly IAuthBusiness _authBusiness;
        private readonly IAuthDA _authDA;
        public AuthController(IAuthBusiness authInstance, IAuthDA authDAInstance) => (_authBusiness, _authDA) = (authInstance, authDAInstance);           
 
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(Users usersdata)
        {
            try
            {
                string client_id = string.Empty;
                string client_secret = string.Empty;
                if (Request.Headers.TryGetValue("clientId", out var clientId)) 
                { client_id = clientId; }
                if (Request.Headers.TryGetValue("clientSecret", out var clientSecret))
                { client_secret = clientSecret; }
                StatusResponse validUser = _authDA.Authenticate(usersdata, client_id, client_secret);
                if (validUser.StatusCode.ToString() != ResponseCode.Success) 
                return Unauthorized(validUser);
                var token = _authBusiness.GenerateToken(usersdata.UserName);
                if (token == null) { return Unauthorized("Invalid Attempt!"); }

                return Ok(token);
            }
            catch (Exception ex)  
            {
                throw;
            }
          
        }
       
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(Tokens token)
        {
            try
            {
                var principal = _authBusiness.GetPrincipalFromExpiredToken(token.Access_Token);
                var username = principal.Identity?.Name;
                Tokens savedRefreshToken = _authBusiness.GetSavedRefreshTokens(username, token.Refresh_Token);
                if (savedRefreshToken == null || savedRefreshToken.Refresh_Token != token.Refresh_Token)                
                   return Unauthorized("Invalid attempt!");                
                var newJwtToken = _authBusiness.GenerateRefreshToken(username);
                if (newJwtToken == null) 
                    return Unauthorized("Invalid attempt!");
                return Ok(newJwtToken);
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

    }
}
