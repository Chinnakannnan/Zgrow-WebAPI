using BusinessModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Report
{
    public interface IReportDA
    {
        List<ReportResponse> PaymentGateWayReport(Reportrequest reportrequest);
        List<ReportResponse> InitiatedId();

    }
}
