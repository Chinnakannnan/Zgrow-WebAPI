using BusinessModel.Admin;
using BusinessModel.Common;
using BusinessModel.PaymentGateway;
using Dapper;
using DataAccess.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common;

namespace DataAccess.Admin
{

    public class AdminDA : DapperRepository<object>, IAdminDA
    {       
        public AdminDA(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {

        }

        public StatusResponse AddCompany(AddAdminRequest addAdminRequest)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@CompanyName", addAdminRequest.CompanyName);
                dParam.Add("@Address1", addAdminRequest.Address1);
                dParam.Add("@Address2", addAdminRequest.Address2);
                dParam.Add("@Address3", addAdminRequest.Address2);
                dParam.Add("@City", addAdminRequest.City);
                dParam.Add("@State", addAdminRequest.State);
                dParam.Add("@Country", addAdminRequest.Country);
                dParam.Add("@PinCode", addAdminRequest.PinCode);
                dParam.Add("@ContactPerson", addAdminRequest.ContactPerson);
                dParam.Add("@ContactMobile1", addAdminRequest.ContactMobile1);
                dParam.Add("@ContactMobile2", addAdminRequest.ContactMobile2);
                dParam.Add("@ContactEmail", addAdminRequest.ContactEmail);
                dParam.Add("@WebURL", addAdminRequest.WebURL);
                dParam.Add("@AadharNumber", addAdminRequest.AadharNumber);
                dParam.Add("@AadharDocumentFront", addAdminRequest.AadharDocumentFront);
                dParam.Add("@AadharDocumentBack", addAdminRequest.AadharDocumentBack);
                dParam.Add("@PANNumber", addAdminRequest.PANNumber);
                dParam.Add("@PANDocument", addAdminRequest.PANDocument);

              StatusResponse dBresult = QuerySP<StatusResponse>("sp_CreateCompany", dParam).FirstOrDefault();
              return dBresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public StatusResponse AddAPI(APIInsert apiInsert)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@APIName", apiInsert.APIName);
                dParam.Add("@SubAction", apiInsert.SubAction);
             
                StatusResponse dBresult = QuerySP<StatusResponse>("sp_APIInsert", dParam).FirstOrDefault();
                return dBresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public StatusResponse AddUser(OnBoardingRequest onBoardingRequest)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@CompanyName", onBoardingRequest.CompanyName);
                dParam.Add("@MasterCompany", onBoardingRequest.MasterCompany);
                dParam.Add("@UserType", onBoardingRequest.UserType);
                dParam.Add("@Address1", onBoardingRequest.Address1);
                dParam.Add("@Address2", onBoardingRequest.Address2);
                dParam.Add("@Address3", onBoardingRequest.Address3);
                dParam.Add("@City", onBoardingRequest.City);
                dParam.Add("@State", onBoardingRequest.State);
                dParam.Add("@Country", onBoardingRequest.Country);
                dParam.Add("@Address1", onBoardingRequest.Address1);
                dParam.Add("@Address2", onBoardingRequest.Address2);
                dParam.Add("@City", onBoardingRequest.City);
                dParam.Add("@State", onBoardingRequest.State);
                dParam.Add("@Country", onBoardingRequest.Country);
                dParam.Add("@Pincode", onBoardingRequest.PinCode);
                dParam.Add("@ContactPerson", onBoardingRequest.ContactPerson);
                dParam.Add("@ContactMobile1", onBoardingRequest.ContactMobile1);
                dParam.Add("@ContactMobile2", onBoardingRequest.ContactMobile2);
                dParam.Add("@ContactEmail", onBoardingRequest.ContactEmail);
                dParam.Add("@WebURL", onBoardingRequest.WebURL);
                dParam.Add("@AadharNumber", onBoardingRequest.AadharNumber);
                dParam.Add("@PANNumber", onBoardingRequest.PANNumber);
                dParam.Add("@AadharDocumentFront", onBoardingRequest.AadharDocumentFront);
                dParam.Add("@AadharDocumentBack", onBoardingRequest.AadharDocumentBack);
                dParam.Add("@PANDocument", onBoardingRequest.PANDocument);

                StatusResponse dBresult = QuerySP<StatusResponse>("sp_onBoarding", dParam).FirstOrDefault();
                return dBresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  List<CompanyList> GetCompanyList()
        {
         try
            {
                var dParam = new DynamicParameters();
                List<CompanyList> dBresult = QuerySP<CompanyList>("sp_getCompanyList", dParam).ToList();
                return dBresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
