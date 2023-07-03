﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Admin
{
    public class AddAdminRequest    { 
    public string CompanyName { get; set; } 
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Address3 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string PinCode { get; set; }
    public string ContactPerson { get; set; }
    public string ContactMobile1 { get; set; }
    public string ContactMobile2 { get; set; }
    public string ContactEmail { get; set; }
    public string WebURL { get; set; }

        public string AadharNumber { get; set; }
        public string PANNumber { get; set; }
        public string AadharDocumentFront { get; set; }
        public string AadharDocumentBack { get; set; }
        public string PANDocument { get; set; }





    }
}
