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


    -----------   Controller OverView End----------------------   */

using BusinessDomain.Admin;
using BusinessDomain.Payout;
using BusinessDomain.User;
using BusinessModel.Admin;
using BusinessModel.Auth;
using BusinessModel.Common;
using BusinessModel.Payout;
using BusinessModel.User;
using DataAccess.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuleEngine.Payout;

namespace NeoBankSolutionAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        string CustomerID = string.Empty;
        private readonly IUserBusiness _businessDomain;
        private readonly CommonDA _LogRepository;
        public UserController(IUserBusiness businessInstance, CommonDA LogInstance) => (_businessDomain, _LogRepository) = (businessInstance, LogInstance);
  
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCompanyList")]
        public async Task<ActionResult> GetCompanyList()
        {
            try
            {
                List<CompanyList> result = _businessDomain.GetCompanyList();
                return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = ResponseMessage.Success, Data = result });

            }
            catch (Exception ex)
            {
                await _LogRepository.ErrorLog(CustomerID, ControllerName.User, MethodName.GetCompanyList, ex.ToString());
                throw;
            }

        }
        [HttpPost]
        [Route("UserInformation")]
        public async Task<IActionResult> UserInformation(UserInfo userInfo)
        {
            try {
                var result = _businessDomain.UserInfo(userInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _LogRepository.ErrorLog(CustomerID, ControllerName.User, MethodName.UserInformation, ex.ToString());
                throw;
            }
            
        }



    }
}
