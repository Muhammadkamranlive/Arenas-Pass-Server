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

        [HttpGet]
        public void Get()
        {
            var document = new PdfDocument();
            var html = @"<!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Invoice</title>
                </head>
                <body style='font-family: Arial, sans-serif; color: #333;'>

                    <div style='text-align: center; margin-bottom: 20px;'>
                        
                        <h1>Invoice</h1>
                    </div>

                    <div style='margin-top: 20px;'>
                        <p>Your Company Name</p>
                        <p>123 Company Street</p>
                        <p>City, State, ZIP</p>
                        <p>Email: info@example.com</p>
                        <p>Phone: (123) 456-7890</p>
                    </div>

                    <table style='width: 100%; border-collapse: collapse; margin-top: 30px;'>
                        <thead>
                            <tr>
                                <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Item Description</th>
                                <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Quantity</th>
                                <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Unit Price</th>
                                <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style='border: 1px solid #ddd; padding: 8px;'>Product 1</td>
                                <td style='border: 1px solid #ddd; padding: 8px;'>2</td>
                                <td style='border: 1px solid #ddd; padding: 8px;'>$50.00</td>
                                <td style='border: 1px solid #ddd; padding: 8px;'>$100.00</td>
                            </tr>
                            <tr>
                                <td style='border: 1px solid #ddd; padding: 8px;'>Product 2</td>
                                <td style='border: 1px solid #ddd; padding: 8px;'>2</td>
                                <td style='border: 1px solid #ddd; padding: 8px;'>$50.00</td>
                                <td style='border: 1px solid #ddd; padding: 8px;'>$100.00</td>
                            </tr>
                            <!-- Add more rows as needed -->
                        </tbody>
                    </table>

                    <div style='margin-top: 20px; text-align: right;'>
                        <p><strong>Total: $100.00</strong></p>
                    </div>

                </body>
                </html>
            ";
            PdfGenerator.AddPdfPages(document, html, PageSize.A4, margin: 40);
            string path = @"C:\\Temp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(path + "\\Test.pdf");
            }
        }
        [HttpPost]
        [Route("GeneratePdf")]
        public IActionResult GeneratePdf([FromBody] PdfDataModel model)
        {
            if (model == null || model.Header == null || model.TableHeaders == null || model.TableData == null)
            {
                return BadRequest("Invalid input data");
            }

            var document = new PdfDocument();
            string imageUrl = "https://arenascards.com/assets/images/land/Logo.png";
            // Build the dynamic HTML
            var html = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>{model.Title}</title>
                </head>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h1>{model.Title}</h1>
                    </div>
                    <div style='text-align: center; margin-bottom: 20px;'>
                   <img src='{imageUrl}' alt='Header Image' style='max-width: 100%; height: auto;'>
                  </div>
                    <div style='margin-top: 20px;'>";

            // Add headers
            foreach (var header in model.Header)
            {
                html += $"<p><strong>{header.Key}:</strong> {header.Value}</p>";
            }

            html += "</div><table style='width: 100%; border-collapse: collapse; margin-top: 30px;'>";

            // Add table headers
            html += "<thead><tr>";
            foreach (var th in model.TableHeaders)
            {
                html += $"<th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>{th}</th>";
            }
            html += "</tr></thead>";

            // Add table data
            html += "<tbody>";
            foreach (var row in model.TableData)
            {
                html += "<tr>";
                foreach (var cell in row)
                {
                    html += $"<td style='border: 1px solid #ddd; padding: 8px;'>{cell}</td>";
                }
                html += "</tr>";
            }
            html += "</tbody></table>";

            // Add footer
            if (!string.IsNullOrEmpty(model.Footer))
            {
                html += $"<div style='margin-top: 20px; text-align: right;'><p><strong>{model.Footer}</strong></p></div>";
            }

            html += "</body></html>";

            // Generate the PDF
            PdfGenerator.AddPdfPages(document, html, PageSize.A4, margin: 40);


            var ms = new MemoryStream();
            document.Save(ms);
            ms.Position = 0; // Reset position to the start of the stream

            // Return the file as a downloadable response
            return File(ms, "application/pdf", "DynamicPdf.pdf");
        }



        [HttpPost]
        [Route("GeneratePdfWithRepeatingHeader")]
        public IActionResult GeneratePdfWithRepeatingHeader([FromBody] PdfDataModel model)
        {
            if (model == null || model.Header == null || model.TableHeaders == null || model.TableData == null)
            {
                return BadRequest("Invalid input data");
            }

            var document = new PdfDocument();

            // Create the HTML structure for the PDF
            var html = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Professional Invoice</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 20px; color: #333;"">
    <!-- Header Section -->
    <div style=""text-align: center; border-bottom: 2px solid #333; margin-bottom: 20px; padding-bottom: 10px;"">
        <img src=""logo.png"" alt=""Company Logo"" style=""max-height: 50px;"">
        <h1 style=""margin: 10px 0;"">Main Company Name</h1>
        <p style=""margin: 5px 0;"">Phone: (123) 456-7890 | Email: info@maincompany.com</p>
    </div>

    <!-- From and To Section -->
    <div>
        <h2 style=""margin-bottom: 5px;"">From:</h2>
        <p style=""margin: 5px 0;"">
            Merchant Company Name<br>
            Merchant Address<br>
            Merchant Email: merchant@example.com
        </p>
        <h2 style=""margin-top: 20px; margin-bottom: 5px;"">To:</h2>
        <p style=""margin: 5px 0;"">
            Customer Name<br>
            Customer Address<br>
            Customer Email: customer@example.com
        </p>
    </div>

    <!-- Table Section -->
    <div style=""margin-top: 20px;"">
        <h2 style=""margin-bottom: 10px;"">Invoice Details</h2>
        <table style=""width: 100%; border-collapse: collapse; margin-top: 10px; border: 1px solid #333;"">
            <thead>
                <tr>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 1</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 2</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 3</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 4</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 5</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 6</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 7</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 8</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 9</th>
                    <th style=""border: 1px solid #333; padding: 8px;"">Header 10</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 1</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 2</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 3</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 4</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 5</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 6</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 7</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 8</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 9</td>
                    <td style=""border: 1px solid #333; padding: 8px;"">Data 10</td>
                </tr>
            </tbody>
        </table>
    </div>

    <!-- Footer Section -->
    <div style=""margin-top: 20px; text-align: center; font-size: 12px;"">
        <p style=""margin: 5px 0;"">Invoice Date: 29-Nov-2024</p>
        <p style=""margin: 5px 0;"">Due Date: 13-Dec-2024</p>
    </div>

    <!-- Signatures Section -->
    <div style=""margin-top: 20"">
        <div style=""text-align: center; width: 30%;"">
            <p>Merchant Signature</p>
            <p>______________________</p>
        </div>
        <div style=""text-align: center; width: 30%;"">
            <p>Customer Signature</p>
            <p>______________________</p>
        </div>
    </div>

    <!-- Regards Section -->
    <div style=""margin-top: 20px; text-align: center; font-size: 12px;"">
        <p>Regards,</p>
        <p>Main Company Name</p>
    </div>
</body>
</html>

";


         
            // Generate the PDF
            PdfGenerator.AddPdfPages(document, html, PageSize.A4, margin:5);

            var ms = new MemoryStream();
            document.Save(ms);
            ms.Position = 0;

            // Return the generated PDF
            return File(ms, "application/pdf", "BankStatement.pdf");
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
        [Route("GeneratePdfWithRepeatForAll")]
        public async Task<dynamic> GeneratePdfWithRepeatForAll([FromBody] PdfDataModel model)
        {
            if (model == null || model.Header == null || model.TableHeaders == null || model.TableData == null)
            {
                return BadRequest("Invalid input data");
            }

            var document = new PdfDocument();

            var html   = await rptService.EmployeeDetailedReportForAll();
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
        public async Task<dynamic> UsersListFormat([FromBody] PdfDataModel model)
        {
            if (model == null || model.Header == null || model.TableHeaders == null || model.TableData == null)
            {
                return BadRequest("Invalid input data");
            }

            var document = new PdfDocument();

            var html   = await rptService.EmployeeReportListFormat();
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
        [Route("SingleGiftCardReport")]
        public async Task<dynamic> SingleGiftCardReport(string SerialNo)
        {
            
            var document = new PdfDocument();

            var html = await rptService.SingleGfitCardReport(SerialNo);
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
