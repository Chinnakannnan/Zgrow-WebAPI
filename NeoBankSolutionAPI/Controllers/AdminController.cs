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
2)This controller for only use Admin Access Purpose

    -----------   Controller OverView End----------------------   */

using BusinessDomain.Admin;
using BusinessDomain.Auth;
using BusinessModel.Admin;
using BusinessModel.Auth;
using BusinessModel.Common;
using DataAccess.Auth;
using DataAccess.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NeoBankSolutionAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        string CustomerID = string.Empty;
        private readonly IAdminBusiness _adminBusiness;
        private readonly CommonDA _LogRepository;
        public AdminController(IAdminBusiness adminInstance, CommonDA LogInstance) => (_adminBusiness, _LogRepository) = (adminInstance, LogInstance);

        [HttpPost]
        [Route("AddCompany")]
        public async Task<ActionResult> AddCompany(AddAdminRequest addAdminRequest)
        {          
            try
            {               
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });

                await _LogRepository.AppLog(CustomerID,ControllerName.Admin, MethodName.AddCompany, JsonConvert.SerializeObject(addAdminRequest).ToString());
               
                // Null Validation
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
                await _LogRepository.ErrorLog(CustomerID,ControllerName.Admin, MethodName.AddCompany, ex.ToString());
                throw;                
            }

        }
        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult>  AddUser(OnBoardingRequest onBoardingRequest)
        { 
            try
            {             
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });

                await _LogRepository.AppLog(CustomerID, ControllerName.Admin, MethodName.AddUser, JsonConvert.SerializeObject(onBoardingRequest).ToString());

                // Null Validation
                if (onBoardingRequest == null)
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Request_Empty });
                // Null Validation (Parameters)
                if (string.IsNullOrEmpty(onBoardingRequest.CompanyName) || string.IsNullOrEmpty(onBoardingRequest.Address1) || string.IsNullOrEmpty(onBoardingRequest.City) || string.IsNullOrEmpty(onBoardingRequest.State) || string.IsNullOrEmpty(onBoardingRequest.Country) || string.IsNullOrEmpty(onBoardingRequest.PinCode) || string.IsNullOrEmpty(onBoardingRequest.ContactPerson) || string.IsNullOrEmpty(onBoardingRequest.ContactMobile1) || string.IsNullOrEmpty(onBoardingRequest.ContactEmail) || string.IsNullOrEmpty(onBoardingRequest.PANNumber) || string.IsNullOrEmpty(onBoardingRequest.PANDocument) || string.IsNullOrEmpty(onBoardingRequest.AadharNumber) || string.IsNullOrEmpty(onBoardingRequest.AadharDocumentFront) || string.IsNullOrEmpty(onBoardingRequest.AadharDocumentBack))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.MandatoryEmpty });
                // PAN Validation
                if (onBoardingRequest.PANNumber.Length.ToString() != "10")
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InvailidPAN });
                // Aadhar Validation
                if (onBoardingRequest.AadharNumber.Length.ToString() != "12")
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InvailidAadhar });
                // Mobile Number Validation
                if (onBoardingRequest.ContactMobile1.Length.ToString() != "10" || onBoardingRequest.ContactMobile2.Length.ToString() != "10")
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.InvailidMobileNumber });
 
                StatusResponse result = _adminBusiness.AddUser(onBoardingRequest);

                if (result.StatusCode == "000")
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc =result.StatusDesc });
                else
                    return Ok(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = result.StatusDesc});

            }
            catch (Exception ex)
            {
                await _LogRepository.ErrorLog(CustomerID,ControllerName.Admin, MethodName.AddUser, ex.ToString());
                throw;
            }

        }
        [HttpPost]
        [Route("ViewOnBoardedUser")]
        public async Task<ActionResult> ViewOnBoardedUser(OnBoardingRequest onBoardUserViewRequest)
        {           
            try
            {
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });

                await _LogRepository.AppLog(CustomerID, ControllerName.Admin, MethodName.AddUser, JsonConvert.SerializeObject(onBoardUserViewRequest).ToString());





                return null;
            }
            catch (Exception ex)
            {
                await _LogRepository.ErrorLog(CustomerID, ControllerName.Admin, MethodName.ViewOnBoardedUser, ex.ToString());
                throw;
            }              

        }       
      
        [HttpPost]
        [Route("AddAPI")]
        public async Task<ActionResult> AddAPI(APIInsert apiInsert)
        {
            try
            {
                if (Request.Headers.TryGetValue("CustomerID", out var customerID))
                {
                    CustomerID = customerID;
                }
                if (string.IsNullOrEmpty(CustomerID))
                    return BadRequest(new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.CustomerID });
                await _LogRepository.AppLog(CustomerID, ControllerName.Admin, MethodName.AddCompany, JsonConvert.SerializeObject(apiInsert).ToString());


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
                await _LogRepository.ErrorLog(CustomerID, ControllerName.Admin, MethodName.AddAPI, ex.ToString());

                throw;
            }

        }

    }
}
