using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    public class CommonDA :DapperRepository<object> 
    {
        
        public CommonDA(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {
        }
        public async Task<bool> AppLog(string ContollerName, string MethodName,string Content)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@Contoller", ContollerName);
                dParam.Add("@Method", MethodName);
                dParam.Add("@Content", Content.ToString());
                var result = await QuerySPTask("sp_Applog", dParam);
               
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> ErrorLog(string ContollerName, string MethodName, string Content)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@Contoller", ContollerName);
                dParam.Add("@Method", MethodName);
                dParam.Add("@Content", Content.ToString());
                var result = await QuerySPTask("sp_ErrorLog", dParam);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
