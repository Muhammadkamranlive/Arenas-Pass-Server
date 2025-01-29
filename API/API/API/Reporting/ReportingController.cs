using PdfSharpCore;
using Server.Models;
using Server.Services;
using PdfSharpCore.Pdf;
using Microsoft.AspNetCore.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace API.API.Reporting
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly IReports_Appple_Passes_Service rptService;
        public ReportingController(IReports_Appple_Passes_Service rpt)
        {
            rptService = rpt;
        }


        [HttpPost]
        [Route("GeneratePdfWithRepeat")]
        public async Task<dynamic> GeneratePdfWithRepeat([FromBody] PdfDataModel model)
        {
            if (model == null || model.Header == null || model.TableHeaders == null || model.TableData == null)
            {
                return BadRequest("Invalid input data");
            }

            var document = new PdfDocument();

            var html = await rptService.EmployeeReport("ec5621a4-e21c-4863-974c-259d8693863b","","","");
            // Generate the PDF
            string htm = (string)html.Response;
            PdfGenerator.AddPdfPages(document,htm, PageSize.A4, margin: 5);

            var ms = new MemoryStream();
            document.Save(ms);
            ms.Position = 0;

            // Return the generated PDF
            return File(ms, "application/pdf", "BankStatement.pdf");
        }


        [HttpPost]
        [Route("EmployeeDetailedReportForAll")]
        public async Task<dynamic> EmployeeDetailedReportForAll(IList<string> EmployeesList )
        {
            
            var document = new PdfDocument();
            var html   = await rptService.EmployeeDetailedReportForAll(EmployeesList);
            if (html.Status_Code != "200")
            {
                return BadRequest(html.Description);
            }
            // Generate the PDF
            string htm = (string)html.Response;
            PdfGenerator.AddPdfPages(document, htm, PageSize.A4, margin: 20);

            var ms = new MemoryStream();
            document.Save(ms);
            ms.Position = 0;

            // Return the generated PDF
            return File(ms, "application/pdf", "BankStatement.pdf");
        }


        [HttpPost]
        [Route("UsersListFormat")]
        public async Task<dynamic> UsersListFormat(IList<string> UsersIdList)
        {
            
            var document                   = new PdfDocument();
            ResponseModel<string> html     = await rptService.EmployeeReportListFormat(UsersIdList);

            if (html.Status_Code != "200")
            {
                return BadRequest(html.Description);
            }

            string htm   = (string)html.Response;
            PdfGenerator.AddPdfPages(document, htm, PageSize.A4, margin: 20);

            var ms       = new MemoryStream();
            document.Save(ms);
            ms.Position = 0;

            // Return the generated PDF
            return File(ms, "application/pdf", "BankStatement.pdf");
        }



        [HttpPost]
        [Route("SingleGiftCardReport")]
        public async Task<dynamic> SingleGiftCardReport(string SerialNo)
        {
            
            var document = new PdfDocument();

            var html = await rptService.SingleGfitCardReport(SerialNo);
            if (html.Status_Code != "200")
            {
                return BadRequest(html.Description);
            }
            // Generate the PDF
            string htm = (string)html.Response;
            PdfGenerator.AddPdfPages(document, htm, PageSize.A4, margin: 20);

            var ms = new MemoryStream();
            document.Save(ms);
            ms.Position = 0;

            // Return the generated PDF
            return File(ms, "application/pdf", "BankStatement.pdf");
        }


        [HttpPost]
        [Route("AllGiftCardReport")]
        public async Task<dynamic> AllGiftCardReport(List<string> SerialNos)
        {

            var document = new PdfDocument();

            var html = await rptService.AllWalletPassesReport(SerialNos);
            if (html.Status_Code != "200")
            {
                return BadRequest(html.Description);
            }
            // Generate the PDF
            string htm = (string)html.Response;
            PdfGenerator.AddPdfPages(document, htm, PageSize.A4, margin: 20);

            var ms = new MemoryStream();
            document.Save(ms);
            ms.Position = 0;

            // Return the generated PDF
            return File(ms, "application/pdf", "BankStatement.pdf");
        }


    }
}
