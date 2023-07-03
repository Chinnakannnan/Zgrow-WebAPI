using BusinessModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Report
{
    public interface IReportBusiness
    {

         List<ReportResponse> PaymentGateWayReport(Reportrequest reportrequest);
        List<ReportResponse> InitiatedId();

    }
}
