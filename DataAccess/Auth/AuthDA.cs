using BusinessModel.Auth;
using BusinessModel.Common;
using BusinessModel.Payout;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Auth
{
    public class AuthDA : DapperRepository<object>, IAuthDA
    {
        public AuthDA(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {

        }


        public StatusResponse Authenticate(Users users, string clientId = null, string clientSecret = null)
        {

            try {
                var dParam = new DynamicParameters();
                dParam.Add("@Username", users.UserName);
                dParam.Add("@Password", users.PassWord);
                dParam.Add("@ClientId", clientId);
                dParam.Add("@ClientSecret", clientSecret);
                var result = QuerySP<StatusResponse>("sp_LoginValidate", dParam).FirstOrDefault();
                 return result;
            }
            catch (Exception ex) {

                throw ex;
            }            
        }
        public StatusResponse TokenUpdate(string Username, String Token,String RefreshToken )
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@Username", Username);
                dParam.Add("@Token", Token);
                dParam.Add("@RefreshToken", RefreshToken);
                var result = QuerySP<StatusResponse>("sp_TokenUpdate", dParam).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Tokens GetRefreshtoken(string Username, String RefreshToken)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@Username", Username);
                dParam.Add("@RefreshToken", RefreshToken);
                var result = QuerySP<Tokens>("sp_GetRefreshToken", dParam).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        


    }

    
}
