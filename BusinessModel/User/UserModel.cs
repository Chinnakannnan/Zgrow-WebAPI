using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.User
{
    public class UserInfo
    {
        [Required]
    public string UserName { get; set; }
    }

    public class UserInfoResponse
    {
        public string UserType { get; set; }
        public string UserName { get; set; }        
        public string CustomerId { get; set; }
        
    }
}



