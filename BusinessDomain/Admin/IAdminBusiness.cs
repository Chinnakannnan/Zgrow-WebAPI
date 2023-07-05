using BusinessModel.Admin;
using BusinessModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Admin
{
    public interface IAdminBusiness
    {
        StatusResponse AddCompany(AddAdminRequest addAdminRequest);
         StatusResponse AddUser(OnBoardingRequest onBoardingRequest);
     
        StatusResponse APIInsert(APIInsert apiRequest);
    }
}
