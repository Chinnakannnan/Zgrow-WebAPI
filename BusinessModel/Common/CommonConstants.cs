using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BusinessModel.Common
{
    public static class CommonConstants
    {
        public const string One = "1";
        public const string Two = "2";
        public const string Three = "3";
        public const string Four = "4";
        public const string Five = "5";
        public const string Six = "6";
        public const string Seven = "7";
        public const string Eight = "8";
    }

    public class ResponseCode
    {
        public const string Success = "000";
        public const string Failed = "001";
        public const string Request_Empty = "002";
        public const string UnAuthorized = "U001";

    }

    public class ResponseMessage
    {
        public const string Success = "Success";
        public const string Failed = "Please Try Again";
        public const string Request_Empty = "Request Empty";
        public const string Company_Code = "Companycode Empty";
        public const string UnExpected = "UnExpected Error";
        public const string InputEmpty = "Please check input parameter. Look like Empty.";
        public const string UnAuthorized = "UnAuthorized Attempt";
        public const string Invalid_ClinetID_Secrect = "Invalid Client / Secrect";
        public const string MandatoryEmpty = "Mandatory value is empty";
        public const string InvailidAadhar = "Invaild Aadhar Number";
        public const string InvailidPAN = "Invaild PAN Number";
        public const string InvailidMobileNumber = "Invailid Mobile Number";
    }

    public class YesBank
    {
        public const string BaseURL = "https://uatskyway.yesbank.in/";
        public const string FundTransfer = "app/uat/api-banking/domestic-payments";
        public const string CertName = "E:\\NeoBankAPI\\NeoBankSolutionAPI\\certificate\\accupayd.p12";
        public const string CheckStatus = "app/uat/api-banking/payment-details";
        public const string Password = "123456";
        public const string ClientID = "9eb2fdc3-688e-4e91-9fb2-d1466c6197d8";
        public const string ClientSecrect = "N4gP0rM4eE7jO4yB4yF3fC7rL2lW1pE0nL5wH1hL7iV8uE4vU3";
    }



    public class EaseBuzz
    {
        public const string BaseURL = "https://pay.easebuzz.in";
        public const string BaseURLStatus = "https://dashboard.easebuzz.in";
        public const string InitiateURL = "/payment/initiateLink";
        public const string CheckStatus = "/transaction/v1/retrieve";
        public const string Key = "L1ZFWH4H4I";
        public const string Salt = "6OZ9P0ZPEW";
        public const string SuccessURL = "https://localhost:7075/PaymentGateway/Success";
        public const string FailureURL = "https://localhost:7075/PaymentGateway/Failure";

    }
}