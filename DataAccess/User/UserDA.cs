﻿using BusinessModel.Admin;
using BusinessModel.Payout;
using BusinessModel.User;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.User
{
    public class UserDA : DapperRepository<object>, IUserDA
    {
        public UserDA(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {
        }
        public List<CompanyList> GetCompanyList()
        {
            try
            {
                var dParam = new DynamicParameters();
                List<CompanyList> dBresult = QuerySP<CompanyList>("sp_getCompanyList", dParam).ToList();
                return dBresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserInfoResponse UserInfo(UserInfo userInfo)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@UserName", userInfo.UserName);          
                var result = QuerySP<UserInfoResponse>("sp_GetUserInfo", dParam).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
   




    }
}
