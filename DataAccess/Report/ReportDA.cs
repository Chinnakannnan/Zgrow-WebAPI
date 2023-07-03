using BusinessModel.Payout;
using BusinessModel.Report;
using Dapper;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Report
{
    public class ReportDA: DapperRepository<object>,IReportDA
    {
        public ReportDA(IDatabaseConfig databaseConfig) : base(databaseConfig)
        {
        } 

        public List<ReportResponse> PaymentGateWayReport(Reportrequest reportrequest)
        {
            try
            {
                var dParam = new DynamicParameters();
                dParam.Add("@CustomerID", reportrequest.CustomerId);
                dParam.Add("@fromDate", reportrequest.FromDate);
                dParam.Add("@toDate", reportrequest.ToDate);
                List<ReportResponse> result = QuerySP<ReportResponse>("sp_GetTxnReport_PaymentGateWay", dParam).ToList(); 
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReportResponse> InitiatedId()
        {
            try
            {
                var dParam = new DynamicParameters(); 

                List<ReportResponse> result = QuerySP<ReportResponse>("sp_getInitiatedId", dParam).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }















    }
}
