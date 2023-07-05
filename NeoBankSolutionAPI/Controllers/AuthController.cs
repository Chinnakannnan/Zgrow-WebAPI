/*  -----------   Controller OverView Start----------------------   
       
1)Should Use Applog and ErrorLog method for log Entry
         Example: 
         try
         {
         await _LogRepository.AppLog(ControllerName.Admin, MethodName.AddUser, "method input".ToString());
         catch (Exception ex)
         {
          await _LogRepository.ErrorLog(ControllerName.Admin, MethodName.AddUser, ex.ToString());
         }
2)This controller for only use Authentication Purpose

    -----------   Controller OverView End----------------------   */

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
using BusinessDomain.Admin;
using DataAccess.Common;
using BusinessModel.Admin;
using Newtonsoft.Json;

namespace NeoBankSolutionAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        string CustomerID = "LoginTry";
        private readonly IAuthBusiness _authBusiness;
        private readonly IAuthDA _authDA;
        private readonly CommonDA _LogRepository; 
        public AuthController(IAuthBusiness authInstance, IAuthDA authDAInstance, CommonDA LogInstance) => (_authBusiness, _authDA, _LogRepository) = (authInstance, authDAInstance, LogInstance);           
 
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
                await _LogRepository.AppLog(clientId, ControllerName.Auth, MethodName.AuthenticateAsync, JsonConvert.SerializeObject(usersdata).ToString());

                if (string.IsNullOrEmpty(client_id))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = "ClientID Empty" });
                if (string.IsNullOrEmpty(client_secret))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = "ClientSecretID Empty" });


                StatusResponse validUser = _authDA.Authenticate(usersdata, client_id, client_secret);
                if (validUser.StatusCode.ToString() != ResponseCode.Success) 
                return Unauthorized(validUser);
                var token = _authBusiness.GenerateToken(usersdata.UserName);
                if (token == null) { return Unauthorized("Invalid Attempt!"); }

                return Ok(token);
            }
            catch (Exception ex)  
            {
                await _LogRepository.ErrorLog(CustomerID,ControllerName.Auth, MethodName.AuthenticateAsync, ex.ToString());
                throw;
            }
          
        }       
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(Tokens token)
        {
            try
            {
                await _LogRepository.AppLog(CustomerID, ControllerName.Auth, MethodName.Refresh, JsonConvert.SerializeObject(token).ToString()); 
               
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
                await _LogRepository.ErrorLog(CustomerID,ControllerName.Auth, MethodName.Refresh, ex.ToString());
                throw;
            }
           
        }

    }
}
