using BusinessModel.Payout;
using DataAccess.Payout; 
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using BusinessModel.Common;


namespace Services_API.Banking
{
    public class BankService :IBankService
    {
        private readonly IPayoutDA _payoutdataservice; 

        public BankService(IPayoutDA instance) => (_payoutdataservice) = (instance);
        public string YesBankPayout(PayoutRequest payoutRequest)
        {
            try
            {
                TransationInsert transationInsert = new TransationInsert();
                YBPTRequest yesbankPaymentRequest = new YBPTRequest();

                var custInfo = _payoutdataservice.CustomerInfo(payoutRequest.CustomerId);
                //------------------------------------------------------//
                transationInsert.CustomerId = payoutRequest.CustomerId;
                transationInsert.ReferenceID = IdGenarator();
                transationInsert.Amount = payoutRequest.Amount;
                transationInsert.TransactionStatus = "1";
                transationInsert.TransactionMode = payoutRequest.Mode;
                transationInsert.TransactionType = "1";//Payout
                transationInsert.TransactionBank = payoutRequest.BankName;
                transationInsert.BeneName = payoutRequest.BeneName;
                transationInsert.BeneAccountNo = payoutRequest.AccountNumber;
                transationInsert.BeneIfsc = payoutRequest.IfscCode;
                //------------------------------------------------------//
                yesbankPaymentRequest.Data.ConsentId = "453733";
                yesbankPaymentRequest.Data.Initiation.InstructionIdentification = transationInsert.ReferenceID;// ReferenceId
                yesbankPaymentRequest.Data.Initiation.EndToEndIdentification = "";
                yesbankPaymentRequest.Data.Initiation.InstructedAmount.Amount = payoutRequest.Amount;
                yesbankPaymentRequest.Data.Initiation.InstructedAmount.Currency = "INR";
                yesbankPaymentRequest.Data.Initiation.DebtorAccount.Identification = "000190600017042";  //001190100016907
                yesbankPaymentRequest.Data.Initiation.DebtorAccount.SecondaryIdentification = "453733";
                yesbankPaymentRequest.Data.Initiation.CreditorAccount.SchemeName = payoutRequest.IfscCode;
                yesbankPaymentRequest.Data.Initiation.CreditorAccount.Identification = payoutRequest.AccountNumber;
                yesbankPaymentRequest.Data.Initiation.CreditorAccount.Name = payoutRequest.BeneName;
                yesbankPaymentRequest.Data.Initiation.CreditorAccount.Unstructured.ContactInformation.EmailAddress = custInfo.EmailAddress;
                yesbankPaymentRequest.Data.Initiation.CreditorAccount.Unstructured.ContactInformation.MobileNumber = custInfo.MobileNo;
                yesbankPaymentRequest.Data.Initiation.RemittanceInformation.Reference = "FRESCO-101";
                yesbankPaymentRequest.Data.Initiation.RemittanceInformation.Unstructured.CreditorReferenceInformation = "RemeToBeneInfo";

                if (payoutRequest.BankName.ToUpper() == "YESBANK")
                {
                    payoutRequest.Mode = "A2A"; // YES to YES
                }
                else
                {
                    if (payoutRequest.Mode == "1")
                    {
                        payoutRequest.Mode = "IMPS"; //IMPS
                    }
                    else if (payoutRequest.Mode == "2")
                    {
                        payoutRequest.Mode = "NEFT"; // NEFT
                    }
                    else if (payoutRequest.Mode == "3")
                    {
                        yesbankPaymentRequest.Data.Initiation.ClearingSystemIdentification = "RTGS"; // RTGS
                    }
                }
                yesbankPaymentRequest.Risk.DeliveryAddress.AddressLine.Add(custInfo.Address1);
                yesbankPaymentRequest.Risk.DeliveryAddress.AddressLine.Add(custInfo.Address2);
                yesbankPaymentRequest.Risk.DeliveryAddress.StreetName = custInfo.City;
                yesbankPaymentRequest.Risk.DeliveryAddress.BuildingNumber = "1";
                yesbankPaymentRequest.Risk.DeliveryAddress.PostCode = custInfo.Pincode;
                yesbankPaymentRequest.Risk.DeliveryAddress.TownName = custInfo.City;
                yesbankPaymentRequest.Risk.DeliveryAddress.CountySubDivision.Add(custInfo.State);
                yesbankPaymentRequest.Risk.DeliveryAddress.Country = "IN";

                string encBankReq = JsonConvert.SerializeObject(yesbankPaymentRequest);
                string lstrBasicAuth = FormingBasicAuth("1057462", "XVMA11wzcq");

                transationInsert.Request = encBankReq.ToString();
                var txnInsert = _payoutdataservice.TxnInsert(transationInsert); // First Entry

                string payoutResponse = YesBankPostAsync(encBankReq);
                // string payoutResponse = POSTData(YESFundTransfer, lstrBasicAuth, encBankReq, "application/json");
                //string payoutResponse = "{\"Data\":{\"ConsentId\":\"453733\",\"TransactionIdentification\":\"f8ea1eb89be211ed87670afa92520000\",\"Status\":\"Received\",\"CreationDateTime\":\"2023-01-24T18:01:05.736+05:30\",\"StatusUpdateDateTime\":\"2023-01-24T18:01:05.736+05:30\",\"Initiation\":{\"InstructionIdentification\":\"YESPOUTB30241801\",\"EndToEndIdentification\":\"\",\"InstructedAmount\":{\"Amount\":\"1000\",\"Currency\":\"INR\"},\"DebtorAccount\":{\"Identification\":\"000190600017042\",\"SecondaryIdentification\":\"453733\"},\"CreditorAccount\":{\"SchemeName\":\"YESB0000262\",\"Identification\":\"026291800001191\",\"Name\":\"Chinnakannan Ajith R\",\"Unstructured\":{\"ContactInformation\":{\"EmailAddress\":\"chinnakannanajithr@gmail.com\",\"MobileNumber\":\"9943535355\"}}},\"RemittanceInformation\":{\"Reference\":\"FRESCO-101\",\"Unstructured\":{\"CreditorReferenceInformation\":\"RemeToBeneInfo\"}},\"ClearingSystemIdentification\":\"FT\"}},\"Risk\":{\"DeliveryAddress\":{\"AddressLine\":[\"test\",\"test\"],\"StreetName\":\"test\",\"BuildingNumber\":\"1\",\"PostCode\":\"41234\",\"TownName\":\"sda\",\"CountySubDivision\":[\"TN\"],\"Country\":\"IN\"}},\"Links\":{\"Self\":\"https:\\/\\/olyuatesbtrans.yesbank.in:7085\\/api-banking\\/v2.0\\/domestic-payments\\/payment-details\"}}";

                if (!payoutResponse.IsNullOrEmpty())
                {
                    YesBankResponse yesBankResponse = JsonConvert.DeserializeObject<YesBankResponse>(payoutResponse);
                         if(yesBankResponse.Data ==null)
                         {
                            return payoutResponse;
                         }
                    if (yesBankResponse.Data.Status == "Received")
                    {
                        transationInsert.Response = payoutResponse.ToString();
                        transationInsert.TransactionStatus = "2";
                        var txnUpdate = _payoutdataservice.TxnUpdate(transationInsert);
                        return payoutResponse.ToString();
                    }
                    else
                    {
                        transationInsert.Response = payoutResponse.ToString();
                        transationInsert.TransactionStatus = "1";
                        var txnUpdate = _payoutdataservice.TxnUpdate(transationInsert);
                        return payoutResponse.ToString();
                    }

                }

                return payoutResponse;
            }
            catch (Exception ex) { return ex.ToString(); }
        }
        public string AxisBankPayout(PayoutRequest payoutRequest)
        {   return null;
        }
         
        public string YesBankPaymentStatus(PayoutCheckRequest payoutCheckRequest)
        {
            TransationInsert transationupdate = new TransationInsert();
            datas jsonRequest = new datas(); 
            CheckStatus jsonRequests = new CheckStatus();
       
            try
            {
                jsonRequest.InstrId = payoutCheckRequest.ReferenceId;
                jsonRequest.ConsentId = "453733";
                jsonRequest.SecondaryIdentification = "453733";

                jsonRequests.Data = jsonRequest;
                string encBankReq = JsonConvert.SerializeObject(jsonRequests);
              

            // string statusResponse = POSTData(YESCheckStatus, lstrBasicAuth, encBankReq, "application/json");

              string statusResponse = YesBankPostAsync(encBankReq);
          // string statusResponse = "{\"Data\":{\"ConsentId\":\"453733\",\"TransactionIdentification\":null,\"Status\":\"FAILED\",\"CreationDateTime\":\"2023-02-06T13:37:33.000+05:30\",\"StatusUpdateDateTime\":\"2023-02-06T13:37:33.000+05:30\",\"Initiation\":{\"InstructionIdentification\":\"YESPOUTP30371337\",\"EndToEndIdentification\":null,\"InstructedAmount\":{\"Amount\":5E+14,\"Currency\":\"INR\"},\"DebtorAccount\":{\"Identification\":\"000190600017042\",\"SecondaryIdentification\":\"453733\"},\"CreditorAccount\":{\"SchemeName\":\"HDFC0000001\",\"Identification\":\"00011020001772\",\"Name\":\"Chinnakannan Ajith R\",\"BeneficiaryCode\":null,\"Unstructured\":{\"ContactInformation\":{\"EmailAddress\":\"chinnakannanajithr@gmail.com\",\"MobileNumber\":\"9943535355\"}},\"RemittanceInformation\":{\"Unstructured\":{\"CreditorReferenceInformation\":\"RemeToBeneInfo\"}},\"ClearingSystemIdentification\":\"NEFT\"}}},\"Risk\":{\"PaymentContextCode\":null,\"DeliveryAddress\":{\"AddressLine\":\"test,test\",\"StreetName\":\"test\",\"BuildingNumber\":\"1\",\"PostCode\":\"41234\",\"TownName\":\"sda\",\"CountySubDivision\":\"TN\",\"Country\":\"IN\"}},\"Links\":{\"Self\":\"https:\\/\\/olyuatesbtrans.yesbank.in:7085\\/api-banking\\/v2.0\\/domestic-payments\\/payment-details\"},\"Meta\":{\"ErrorCode\":\"ns:E402\",\"ErrorSeverity\":\"Wrong data in Cheque No field LogRes\",\"ActionDescription\":\"FAILED\"}}";

                YesBankResponse responseStatus = JsonConvert.DeserializeObject<YesBankResponse>(statusResponse);
                //----
                transationupdate.CustomerId = payoutCheckRequest.CustomerId;
                transationupdate.ReferenceID= payoutCheckRequest.ReferenceId;
                transationupdate.TransactionStatus= responseStatus.Data.Status;
                transationupdate.Request = encBankReq.ToString();
                transationupdate.Response = statusResponse.ToString();
                //----
                var txnInsert = _payoutdataservice.TxnUpdateStatus(transationupdate);


                return statusResponse;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

               }

        private string FormingBasicAuth(string lstrUserName, string lstrPassword)
        {
            try { return "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(lstrUserName + ":" + lstrPassword)); }
            catch (Exception ex) { return ""; } 
        }
        public string IdGenarator()
        {
            DateTime d = DateTime.Now;
            string ms = DateTime.Now.ToString("HHmm");
            int julian = d.Year * 1000 + d.DayOfYear;
            string julian2 = julian.ToString();
            string convertJulian = julian2.Substring(julian2.Length - 4);
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string alphanumaric = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
            string alphanumaric2 = new string(Enumerable.Repeat(chars, 4) .Select(s => s[random.Next(s.Length)]).ToArray());
            string id = "P" + alphanumaric + alphanumaric2 + convertJulian + ms;
            return id;

        }


        public string YesBankPostAsync(string body)
        {
            try {
                string lstrBasicAuth = FormingBasicAuth("testuser", "TiEsbntp@13N22");
                HttpClient httpClient = new HttpClient();
                var handler = new HttpClientHandler();
                httpClient.BaseAddress = new Uri(YesBank.BaseURL);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", lstrBasicAuth);
                httpClient.DefaultRequestHeaders.Add("X-IBM-Client-Id", "Bearer " + YesBank.ClientID);
                httpClient.DefaultRequestHeaders.Add("X-IBM-Client-Secret", "Bearer " + YesBank.ClientSecrect);
                var stringContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, ("application/json"));
                HttpResponseMessage response = httpClient.PostAsync(YesBank.FundTransfer, stringContent).Result;

                X509Certificate2 certificate = new X509Certificate2(YesBank.CertName, "123456");
                handler.ClientCertificates.Add(certificate);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    return responseBody;
                }
                string responseFailed = response.Content.ReadAsStringAsync().Result;
                return responseFailed;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
