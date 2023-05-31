using BusinessDomain.Payout;
using BusinessDomain.User;
using BusinessModel.Auth;
using BusinessModel.Payout;
using BusinessModel.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuleEngine.Payout;

namespace NeoBankSolutionAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _businessDomain;  

        public UserController(IUserBusiness businessInstance) => (_businessDomain) = (businessInstance);


        [HttpPost]
        [Route("UserInformation")]
        public IActionResult UserInformation(UserInfo userInfo)
        {
            var result = _businessDomain.UserInfo(userInfo);            

                return Ok(result);
        }



    }
}
