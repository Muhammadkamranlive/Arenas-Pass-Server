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
        Task<ResponseModel<string>> EmployeeDetailedReportForAll();
        Task<ResponseModel<string>> EmployeeReportListFormat();
        Task<ResponseModel<string>> SingleGfitCardReport(string SerialNo);
        Task<ResponseModel<string>> AllWalletPassesReport(List<string> SerialNos);
        Task<ResponseModel<string>> EmployeeReport(string EmployeeId, string AccountDetail, string CompanyDetail, string ContactInfo);

    }
}
