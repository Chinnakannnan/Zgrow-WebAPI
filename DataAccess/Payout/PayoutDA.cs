using BusinessModel.Common;
using BusinessModel.Payout;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataAccess.Payout
{
    public class PayoutDA : DapperRepository<object>, IPayoutDA
    {
        public PayoutDA(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {
        }

        public PrimaryCheckModel PrimaryCheck(string APIName, String CompanyCode, string CustomerId)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@APIName", APIName);
                dParam.Add("@CompanyCode", CompanyCode);
                dParam.Add("@CustomerId", CustomerId);
                var result = QuerySP<PrimaryCheckModel>("sp_PrimaryCheck", dParam).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserModel CustomerInfo(string CustomerId)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@CustomerId", CustomerId);
                var result = QuerySP<UserModel>("sp_GetUser", dParam).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> TxnInsert(TransationInsert transationInsert)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@CustomerId", transationInsert.CustomerId);
                dParam.Add("@ReferenceID", transationInsert.ReferenceID);
                dParam.Add("@Amount", transationInsert.Amount);
                dParam.Add("@TransactionStatus", "1"); // Initiated
                dParam.Add("@TransactionMode", transationInsert.TransactionMode);
                dParam.Add("@TransactionType", transationInsert.TransactionType);
                dParam.Add("@TransactionBank", transationInsert.TransactionBank);
                dParam.Add("@BeneName", transationInsert.BeneName);
                dParam.Add("@BeneAccountNo", transationInsert.BeneAccountNo);
                dParam.Add("@BeneIfsc", transationInsert.BeneIfsc);
                dParam.Add("@IsActive", "1");
                dParam.Add("@Request", transationInsert.Request);
                var result = await QuerySPTask("sp_InsertTxn", dParam);              
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> TxnUpdate(TransationInsert transationInsert)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@CustomerId", transationInsert.CustomerId);
                dParam.Add("@ReferenceID", transationInsert.ReferenceID);             
                dParam.Add("@TransactionStatus", transationInsert.TransactionStatus);
                dParam.Add("@Response", transationInsert.Response);
                var result = await QuerySPTask("sp_UpdateTxn", dParam);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> TxnUpdateStatus(TransationInsert transationInsert)
        {
            try
            { 
                var dParam = new DynamicParameters();
                dParam.Add("@CustomerId", transationInsert.CustomerId);
                dParam.Add("@ReferenceID", transationInsert.ReferenceID);
                dParam.Add("@TransactionStatus", transationInsert.TransactionStatus);
                dParam.Add("@Request", transationInsert.Request);
                dParam.Add("@Response", transationInsert.Response);
                var result = await QuerySPTask("sp_UpdateTxn", dParam);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
