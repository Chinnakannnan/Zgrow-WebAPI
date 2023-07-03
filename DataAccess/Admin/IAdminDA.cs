using BusinessModel.Admin;
using BusinessModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Admin
{
    public interface IAdminDA
    {
    StatusResponse AddCompany(AddAdminRequest addAdminRequest);
    StatusResponse AddUser(OnBoardingRequest onBoardingRequest);
     List<CompanyList> GetCompanyList();
     StatusResponse AddAPI(APIInsert apiInsert);
    }
}
