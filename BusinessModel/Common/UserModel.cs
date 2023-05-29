using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Common
{
    public class UserModel
    {
        public string UserID { get; set; }
        public string CustomerId { get; set; }
        public string CompanyCode { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Tpin { get; set; }
        public string UserType { get; set; }
        public string Remarks { get; set; }
        public string AesKey { get; set; }
        public string Consumerkey { get; set; }
        public string ConsumerSecret { get; set; }
        public string Password { get; set; }
        public string passwordcount { get; set; }
        public string passwordexpiry { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedTime { get; set; }
        public string ModifiedTime { get; set; }
        public string ModifiedBy { get; set; }

    }
}
