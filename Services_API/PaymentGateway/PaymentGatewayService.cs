using BusinessModel.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using BusinessModel.Common; 
using System.Security.Cryptography;
using System.Security.Policy;
using System.Numerics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using XAct.Messages;
using DataAccess.Payout;
using DataAccess.PaymentGateway;
using Microsoft.IdentityModel.Abstractions;
using MediatR;
using System.Net;

namespace Services_API.PaymentGateway
{
  
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly IPaymentGatewayDA _payoutdataservice;
        public PaymentGatewayService(IPaymentGatewayDA instance) => (_payoutdataservice) = (instance);

        public LinkResponse EasebuzzPaymentInitiate(InitiateRequest initiateRequest)
        {
            string udf1 = string.Empty;
            string udf2 = string.Empty;
            string udf3 = string.Empty;
            string udf4 = string.Empty;
            string udf5 = string.Empty;
            string udf6 = string.Empty;
            string udf7 = string.Empty;
            string udf8 = string.Empty;
            string udf9 = string.Empty;
            string udf10 = string.Empty;

            var TxnId = IdGenarator();

            string hashVarsSeq = EaseBuzz.Key + "|" + TxnId + "|" + initiateRequest.Amount + "|" + initiateRequest.Description + "|" + initiateRequest.Name + "|"
                                      + initiateRequest.MailId + "|" + udf1 + "|" + udf2 + "|" + udf3 + "|" + udf4 + "|" + udf5 + "|" + udf6 + "|" + udf7 + "|"
                                      + udf8 + "|" + udf9 + "|" + udf10 + "|" + EaseBuzz.Salt;

            var gen_hash = Easebuzz_Generatehash512(hashVarsSeq).ToLower();

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("txnid", TxnId);
            dict.Add("key", EaseBuzz.Key);
            dict.Add("amount", initiateRequest.Amount.Trim());
            dict.Add("firstname", initiateRequest.Name.Trim());
            dict.Add("email", initiateRequest.MailId.Trim());
            dict.Add("phone", initiateRequest.MobileNumber.Trim());
            dict.Add("productinfo", initiateRequest.Description.Trim());
            dict.Add("surl", EaseBuzz.SuccessURL.Trim());
            dict.Add("furl", EaseBuzz.FailureURL.Trim());
            dict.Add("udf1", udf1.Trim());
            dict.Add("udf2", udf2.Trim());
            dict.Add("udf3", udf3.Trim());
            dict.Add("udf4", udf4.Trim());
            dict.Add("udf5", udf5.Trim());
            dict.Add("udf6", udf6.Trim());
            dict.Add("udf7", udf7.Trim());
            dict.Add("udf8", udf8.Trim());
            dict.Add("udf9", udf9.Trim());
            dict.Add("udf10", udf10.Trim());
            dict.Add("hash", gen_hash);

            string result = EaseBuzzPostAsync(dict);

             _payoutdataservice.TxnInitiate(dict, TxnId, initiateRequest, result);// Insert Request and Responses

            LinkResponse value = new LinkResponse();
            value.TxnID = TxnId;
            value.data = result;
            return value;

        }

        public string EasebuzzCheckststatus(GetStatusRequest getStatusRequest)
        {
            BasicTxnInfo values =_payoutdataservice.GetTxnBasicInfo(getStatusRequest); 
            string hashVarsSeq = EaseBuzz.Key + "|" + getStatusRequest.txnID + "|" + values.Amount + "|" + values.MailId + "|" + values.BeneMobileNumber + "|" + EaseBuzz.Salt;
            string gen_hash = ComputeSHA512Hash(hashVarsSeq); 
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("txnid", getStatusRequest.txnID);
            dict.Add("key", EaseBuzz.Key);
            dict.Add("amount", values.Amount);
            dict.Add("email", values.MailId);
            dict.Add("phone", values.BeneMobileNumber);
            dict.Add("hash", gen_hash);  
            var httpClient = new HttpClient();
            var handler = new HttpClientHandler();
            httpClient.BaseAddress = new Uri(EaseBuzz.BaseURLStatus);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var collection = new List<KeyValuePair<string, string>>();
            foreach (var datas in dict)
            {
                collection.Add(new(datas.Key, datas.Value));
            }
            var content = new FormUrlEncodedContent(collection);
            HttpResponseMessage response = httpClient.PostAsync(EaseBuzz.CheckStatus, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                 
                return responseBody ;
            }
            return "";

        }
     
 
       public string Easebuzz_Generatehash512(string text) //payment initiate
         {
            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static string ComputeSHA512Hash(string input) // get status
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
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
            string alphanumaric2 = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
            string id = "Link" + alphanumaric + alphanumaric2 + convertJulian + ms;
            return id;

        }

        public string EaseBuzzPostAsync(Dictionary<string,string> dict)
        {
            try
            {
                var httpClient = new HttpClient();
                var handler = new HttpClientHandler();
                httpClient.BaseAddress = new Uri(EaseBuzz.BaseURL);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var collection = new List<KeyValuePair<string, string>>();
                foreach (var data in dict)
                {
                    collection.Add(new(data.Key, data.Value));
                }
                var content = new FormUrlEncodedContent(collection); 
                
                HttpResponseMessage response = httpClient.PostAsync(EaseBuzz.InitiateURL, content).Result;


                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                   

                    return responseBody;

                }
                string responseFailed = response.Content.ReadAsStringAsync().Result;
                return responseFailed;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
