using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    public class Common :DapperRepository<object>
    {
        public Common(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {
        }
        public async Task<bool> AppLog(string Content)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@Content", Content);
                var result = await QuerySPTask("AppLog", dParam);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> ErrorLog(string Content)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@Content", Content);
                var result = await QuerySPTask("ErrorLog", dParam);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
