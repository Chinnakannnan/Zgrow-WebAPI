using BusinessDomain.Admin;
using BusinessDomain.Auth;
using BusinessModel.Admin;
using BusinessModel.Auth;
using BusinessModel.Common;
using DataAccess.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NeoBankSolutionAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminBusiness _adminBusiness; 
        public AdminController(IAdminBusiness adminInstance) => (_adminBusiness) = (adminInstance);


        [HttpPost]
        [Route("AddCompany")]
        public IActionResult AddCompany(AddAdminRequest addAdminRequest)
        {
            try
            {    // Null Validation
                if (addAdminRequest == null) 
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });
                // Null Validation (Parameters)
                if (string.IsNullOrEmpty(addAdminRequest.CompanyName) || string.IsNullOrEmpty(addAdminRequest.Address1) || string.IsNullOrEmpty(addAdminRequest.City) || string.IsNullOrEmpty(addAdminRequest.State) || string.IsNullOrEmpty(addAdminRequest.Country) || string.IsNullOrEmpty(addAdminRequest.PinCode) || string.IsNullOrEmpty(addAdminRequest.ContactPerson) || string.IsNullOrEmpty(addAdminRequest.ContactMobile1) || string.IsNullOrEmpty(addAdminRequest.ContactEmail) || string.IsNullOrEmpty(addAdminRequest.PANNumber) || string.IsNullOrEmpty(addAdminRequest.PANDocument)|| string.IsNullOrEmpty(addAdminRequest.AadharNumber) || string.IsNullOrEmpty(addAdminRequest.AadharDocumentFront) || string.IsNullOrEmpty(addAdminRequest.AadharDocumentBack))
                   return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.MandatoryEmpty });
                 // PAN Validation
                if (addAdminRequest.PANNumber.Length.ToString() != "10")
                        return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InvailidPAN });
                // Aadhar Validation
                if (addAdminRequest.AadharNumber.Length.ToString() != "12")
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InvailidAadhar });
                // Mobile Number Validation
                if (addAdminRequest.ContactMobile1.Length.ToString() != "10"|| addAdminRequest.ContactMobile2.Length.ToString() != "10")
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InvailidMobileNumber });


                 StatusResponse result = _adminBusiness.AddCompany(addAdminRequest);
                if(result.StatusCode=="000")
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = result.StatusDesc});
                else
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = result.StatusDesc });

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(OnBoardingRequest onBoardingRequest)
        {
            try
            {
                if (onBoardingRequest == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });
                if (string.IsNullOrEmpty(onBoardingRequest.ContactMobile1) || string.IsNullOrEmpty(onBoardingRequest.ContactEmail))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.MandatoryEmpty });
                StatusResponse result = _adminBusiness.AddUser(onBoardingRequest);

                if (result.StatusCode == "000")
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc =result.StatusDesc });
                else
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = result.StatusDesc});

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetCompanyList")]
        public IActionResult GetCompanyList()
        {
            try
            {
                List<CompanyList> result = _adminBusiness.GetCompanyList();
                return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = ResponseMessage.Success, Data = result });

            }
            catch (Exception ex)
            {
                throw;
            }

        }
      
        [HttpPost]
        [Route("AddAPI")]
        public IActionResult AddAPI(APIInsert apiInsert)
        {
            try
            {
                if (apiInsert == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });
                if (string.IsNullOrEmpty(apiInsert.APIName) || string.IsNullOrEmpty(apiInsert.SubAction))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.MandatoryEmpty });
                StatusResponse result = _adminBusiness.APIInsert(apiInsert);

                if (result.StatusCode == "000")
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = result.StatusDesc });
                else
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = result.StatusDesc });

            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
