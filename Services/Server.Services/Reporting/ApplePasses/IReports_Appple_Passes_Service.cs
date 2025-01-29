using System;
using System.Linq;
using System.Text;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IReports_Appple_Passes_Service
    {
        MemoryStream GenerateReport();
        Task<ResponseModel<string>> EmployeeDetailedReportForAll(IList<string> EMPIDs);
        Task<ResponseModel<string>> EmployeeReportListFormat(IList<string> EmployeeIds);
        Task<ResponseModel<string>> SingleGfitCardReport(string SerialNo);
        Task<ResponseModel<string>> AllWalletPassesReport(List<string> SerialNos);
        Task<ResponseModel<string>> EmployeeReport(string EmployeeId, string AccountDetail, string CompanyDetail, string ContactInfo);

    }
}
