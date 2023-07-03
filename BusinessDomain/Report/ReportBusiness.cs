using BusinessModel.Payout;
using BusinessModel.Report;
using DataAccess.Report;
using DataAccess.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Report{
        public class ReportBusiness: IReportBusiness  
    {

        private readonly IReportDA _reportRepository;
        public ReportBusiness(IReportDA reportInstance) => (_reportRepository) = (reportInstance);


        public List<ReportResponse> PaymentGateWayReport(Reportrequest reportrequest)
        {
            var result = _reportRepository.PaymentGateWayReport(reportrequest);
            return result;
        }


        public List<ReportResponse> InitiatedId()
        {
            var result = _reportRepository.InitiatedId();
            return result;
        }
 














    }
}
