using System;
using System.Linq;
using System.Text;
using PdfSharpCore;
using Server.Models;
using Server.Domain;
using PdfSharpCore.Pdf;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Server.Services
{
    public class Reports_Appple_Passes_Service : IReports_Appple_Passes_Service
    {
        private readonly IAuthManager                 _authManager;
        private readonly ITenants_Service             _tenantsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGift_Card_Service           _giftCardService;
        public Reports_Appple_Passes_Service
        (
                IAuthManager                 auth,
                ITenants_Service             tenantsService,
                UserManager<ApplicationUser> userManager,
                IGift_Card_Service           card_Service
        )
        {
            _authManager     = auth;
            _tenantsService  = tenantsService;
            _userManager     = userManager;
            _giftCardService = card_Service;
        }
        public async Task<ResponseModel<string>> EmployeeReport(string EmployeeId,string AccountDetail,string CompanyDetail,string ContactInfo)
        {
            try
            {
                ResponseModel<string> PdfResponse = new ResponseModel<string>() {Status_Code="200",Description="OK" };
                //Account Information
                ApplicationUser  UserDetail = await _authManager.FindById(EmployeeId);
                if (UserDetail == null) 
                {
                    PdfResponse.Status_Code = "404";
                    PdfResponse.Description = "No Account Detail Record Found";
                    return PdfResponse;
                }
                //Roles
                var roles = _userManager.GetRolesAsync(UserDetail);
                if (roles.Result.Count==0)
                {
                    PdfResponse.Status_Code = "404";
                    PdfResponse.Description = "User Role is not defined please assign him role";
                    return PdfResponse;
                }
                //Company Information
                ArenasTenants arenasTenants = await _tenantsService.FindOne(x=>x.CompanyId==UserDetail.TenantId);
                if (UserDetail == null)
                {
                    PdfResponse.Status_Code = "404";
                    PdfResponse.Description = "Company details cannot be found";
                    return PdfResponse;
                }

                //Html Pdf
                string html = $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Professional Invoice</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            margin: 20px;
                            color: #333;
                        }}
                        .header {{
                            text-align: center;
                            border-bottom: 2px solid #333;
                            margin-bottom: 20px;
                            padding-bottom: 10px;
                        }}
                        .header img {{
                            max-height: 50px;
                        }}
                        .details, .from-to, .table-section, .footer {{
                            margin-bottom: 20px;
                        }}
                        table {{
                            width: 100%;
                            border-collapse: collapse;
                        }}
                        table, th, td {{
                            border: 1px solid #333;
                        }}
                        th, td {{
                            text-align: left;
                        }}
                        .footer {{
                            text-align: center;
                            font-size: 12px;
                        }}
                        .signature-section {{
                            display: flex;
                            justify-content: space-between;
                            margin-top: 20px;
                        }}
                        .signature {{
                            text-align: center;
                            width: 30%;
                        }}
                    </style>
                </head>
                <body>
                    <!-- Header Section -->
                    <div class=""header"">
                        <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Logo.png?alt=media&token=e645be03-ac1f-4b28-8aea-4ab2a7c6ea61"" alt=""Company Logo"" style=""height: 50px;"">
                        <p style=""margin-top:-5px"">Phone: (123) 456-7890 | Email: info@maincompany.com</p>
                    </div>
   
                    <table style=""border: none;"">
                        <tr style=""border: none;"">
                            <td style=""border: none;"">
                                <h4>User account detail</h4>
                            </td>
                        </tr>
                    </table>
                    <table >
                        <thead>
                            <tr>
                               
                                <th>User</th>
                                <th>Company</th>
                                <th>Email</th>
                                <th>Access Role</th>
                                <th>Company Designation</th>
                                <th>App Id</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                
                                <td>{UserDetail.FirstName} {UserDetail.LastName}</td>
                                <td>{UserDetail.CompanyName}</td>
                                <td>
                                 {UserDetail.Email}
                                </td>
                                <td>
                                 {roles.Result.FirstOrDefault()}
                                </td>
                                <td>
                                 {UserDetail.CompanyDesignation}
                                </td>
                                <td>
                                 {UserDetail.EmployeeId}
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table style=""border: none;"">
                        <tr style=""border: none;"">
                            <td style=""border: none;"">
                                <h4>User's company detail</h4>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <thead>
                            <tr>
                                <th>User           </th>
                                <th>Company        </th>
                                <th>Phone          </th>
                                <th>Tenant Id      </th>
                                <th>Company Status </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                {UserDetail.FirstName} {UserDetail.LastName}
                                </td>
                                <td>
                                 {arenasTenants.CompanyName}
                                </td>
                                <td>{arenasTenants.PhoneNumber}</td>
                                <td>{arenasTenants.CompanyId}</td>
                                <td>{arenasTenants.CompanyStatus}</td>
                            </tr>
                        </tbody>
                    </table>
                    <table style=""border: none;"">
                        <tr style=""border: none;"">
                            <td style=""border: none;"">
                                <h4>User's company location detail</h4>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <thead>
                            <tr>
                                <th>Company </th>
                                <th>City    </th>
                                <th>Country </th>
                                <th>Address </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>{arenasTenants.CompanyName}</td>
                                <td>{arenasTenants.City}</td>
                                <td>{arenasTenants.Country}</td>
                                <td>{arenasTenants.AddressLine1}</td>
                            </tr>
                        </tbody>
                    </table>

                    <!-- Regards Section -->
                    <div class=""footer"">
                        <h1>Regards,</h1>
                        <h2>Arenas Pass</h2>
                        Sign_____________________
                    </div>
                </body>
                </html>
                ";

                PdfResponse.Status_Code = "200";
                PdfResponse.Response    = html;
                PdfResponse.Description = html;
                return PdfResponse;
            }
            catch (Exception ex)
            {
               ResponseModel<string> response= new ResponseModel<string>()
               {
                   Status_Code="500",
                   Description = ex.Message,
               };
               return response;
            }
        }


        public async Task<ResponseModel<string>> EmployeeDetailedReportForAll(IList<string> EMPIDs)
        {
            try
            {
                // Initialize Response
                var response = new ResponseModel<string> { Status_Code = "200", Description = "OK" };

                // Fetch all employees
                IList<AllUsersModel> allUsers = await _authManager.GetUsersListByIds(EMPIDs);
                if (!allUsers.Any())
                {
                    return new ResponseModel<string> { Status_Code = "404", Description = "No employees found" };
                }

                // Start HTML Builder
                var htmlBuilder = new StringBuilder();
                htmlBuilder.Append(@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>All Employees Report</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    margin: 20px;
                    color: #333;
                }
                .header {
                    text-align: center;
                    border-bottom: 2px solid #333;
                    margin-bottom: 20px;
                    padding-bottom: 10px;
                }
                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                }
                th, td {
                    border: 1px solid #333;
                    padding: 8px;
                    text-align: left;
                }
                th {
                    background-color: #f2f2f2;
                }
                .footer {
                    text-align: center;
                    font-size: 12px;
                }
            </style>
        </head>
        <body>
        <div class=""header"">
            <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Logo.png?alt=media&token=e645be03-ac1f-4b28-8aea-4ab2a7c6ea61"" alt=""Company Logo"" style=""height: 50px;"">
            <p style=""margin-top:-5px"">Phone: (123) 456-7890 | Email: info@maincompany.com</p>
        </div>");

                // Iterate over employees
                foreach (var user in allUsers)
                {
                    var tenant = await _tenantsService.FindOne(x => x.CompanyName == user.CompanyName);

                    htmlBuilder.Append($@"
            <h4>User Account Detail</h4>
            <table>
                <thead>
                    <tr>
                        <th>Profile</th>
                        <th>User</th>
                        <th>Company</th>
                        <th>Email</th>
                        <th>Access Role</th>
                        <th>App ID</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><img src=""{user.Image}"" alt=""Company Logo"" style=""height: 50px;width:50px""></td>
                        <td>{user.FirstName} {user.LastName}</td>
                        <td>{user.CompanyName}</td>
                        <td>{user.Email}</td>
                        <td>{user.Roles}</td>
                        <td>{user.EmployeeId}</td>
                    </tr>
                </tbody>
            </table>
            <h4>User's Company Detail</h4>
            <table>
                <thead>
                    <tr>
                        <th>User</th>
                        <th>Company</th>
                        <th>Phone</th>
                        <th>Tenant ID</th>
                        <th>Company Status</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>{user.FirstName} {user.LastName}</td>
                        <td>{tenant?.CompanyName ?? "N/A"}</td>
                        <td>{tenant?.PhoneNumber ?? "N/A"}</td>
                        <td>{tenant.CompanyId }</td>
                        <td>{tenant?.CompanyStatus ?? "N/A"}</td>
                    </tr>
                </tbody>
            </table>
            <h4>User's Company Location Detail</h4>
            <table style=""margin-bottom:20px"">
                <thead>
                    <tr>
                        <th>Company</th>
                        <th>City</th>
                        <th>Country</th>
                        <th>Address</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>{tenant?.CompanyName ?? "N/A"}</td>
                        <td>{tenant?.City ?? "N/A"}</td>
                        <td>{tenant?.Country ?? "N/A"}</td>
                        <td>{tenant?.AddressLine1 ?? "N/A"}</td>
                    </tr>
                </tbody>
            </table>
            <hr />"
            );
                }

                // Close HTML
                htmlBuilder.Append(@"
                   <table style=""width:100%; margin-top: 50px;bordor:none;"">
                   <tr>
                   <td style=""text-align:right;"">
                       <div style=""background-color:#f2f2f2; display:inline-block; padding:10px; border-radius:5px;"">
                           <strong>REMIT TO</strong>
                       </div>
                       <div style=""text-align:right; margin-top:10px;"">
                           Arenas Pass Inc.<br>
                           123 Business St., Suite 456<br>
                           New York, NY 10001<br>
                           USA
                       </div>
                   </td>
                  </tr>
                 </table>

                
                ");

                response.Response = htmlBuilder.ToString();
                response.Description = "Report generated successfully";
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message };
            }
        }


        public MemoryStream GenerateReport()
        {
			try
			{

                var document = new PdfDocument();
                var html = $@"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Professional Invoice</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            margin: 20px;
                            color: #333;
                        }}
                        .header {{
                            text-align: center;
                            border-bottom: 2px solid #333;
                            margin-bottom: 20px;
                            padding-bottom: 10px;
                        }}
                        .header img {{
                            max-height: 50px;
                        }}
                        .details, .from-to, .table-section, .footer {{
                            margin-bottom: 20px;
                        }}
                        table {{
                            width: 100%;
                            border-collapse: collapse;
                        }}
                        table, th, td {{
                            border: 1px solid #333;
                        }}
                        th, td {{
                            padding: 4px;
                            text-align:center;
                        }}
                        .footer {{
                            text-align: center;
                            font-size: 12px;
                        }}
                        .signature-section {{
                            display: flex;
                            justify-content: space-between;
                            margin-top: 20px;
                        }}
                        .signature {{
                            text-align: center;
                            width: 30%;
                        }}
                        .from-to td {{
                          text-align:left !important
                        }}
                    </style>
                   </head>
                   <body>
                       <!-- Header Section -->
                 <div class=""header"">
                     <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Logo.png?alt=media&token=e645be03-ac1f-4b28-8aea-4ab2a7c6ea61"" alt=""Company Logo"" style=""height: 50px;"">
                     <h1>Arenas Pass</h1>
                     <p>Phone: (123) 456-7890 | Email: info@maincompany.com</p>
                 </div>
                <table style=""border: none;"" class=""from-to"">
                 <thead>
                     <tr style=""border: none;"">
                      <td style=""border: none;"">From</td>
                      <td style=""border: none;"">To</td>
                     </tr>
                 </thead> 
                 <tbody>
                <tr>
            <td style=""border: none;"">
                <table style=""border: none;"" class=""from-to"">
                    <tr>
                        <td style=""border: none;"">
                            Merchant Company Name
                        </td>
                    </tr>
                    <tr>
                        <td style=""border: none;"">
                            Merchant Address
                        </td>
                    </tr>
                    <tr>
                        <td style=""border: none;"">
                            Merchant Email: merchant@example.com
                        </td>
                    </tr>
                </table>
            </td>
            <td style=""border: none;"" class=""from-to"">
                <table style=""border: none;"">
                    <tr>
                        <td style=""border: none;"">
                            Merchant Company Name
                        </td>
                    </tr>
                    <tr>
                        <td style=""border: none;"">
                            Merchant Address
                        </td>
                    </tr>
                    <tr>
                        <td style=""border: none;"">
                            Merchant Email: merchant@example.com
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </tbody>
   </table>
    <!-- Table Section -->
    <div class=""table-section"">
        <h2>Invoice Details</h2>
        <table>
            <thead>
                <tr>
                    <th>Header 1</th>
                    <th>Header 2</th>
                    <th>Header 3</th>
                    <th>Header 4</th>
                    <th>Header 5</th>
                    <th>Header 6</th>
                    <th>Header 7</th>
                    <th>Header 8</th>
                    <th>Header 9</th>
                    <th>Header 10</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Data 1</td>
                    <td>Data 2</td>
                    <td>Data 3</td>
                    <td>Data 4</td>
                    <td>Data 5</td>
                    <td>Data 6</td>
                    <td>Data 7</td>
                    <td>Data 8</td>
                    <td>Data 9</td>
                    <td>Data 10</td>
                </tr>
            </tbody>
        </table>
    </div>

    <!-- Regards Section -->
    <div class=""footer"">
        <p>Regards,</p>
        <p>Main Company Name</p>
    </div>
</body>
</html>
";



                // Generate the PDF
                PdfGenerator.AddPdfPages(document, html, PageSize.A4, margin: 5);

                var ms = new MemoryStream();
                document.Save(ms);
                ms.Position = 0;
                return ms;
            }
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
        }




        /// <summary>
        /// Employees List Style Export
        /// </summary>
        /// <param name="EmployeeIds"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> EmployeeReportListFormat(IList<string> EmployeeIds)
        {
            try
            {
                // Initialize Response
                var response = new ResponseModel<string> { Status_Code = "200", Description = "OK" };

                // Fetch all employees
                var  allUsers = await _authManager.GetUsersListByIds(EmployeeIds);

                if (!allUsers.Any())
                {
                    return new ResponseModel<string> { Status_Code = "404", Description = "No user found" };
                }

               
               

                // Start HTML Builder
                var htmlBuilder = new StringBuilder();
                htmlBuilder.Append(@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>All Employees Report</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    margin: 20px;
                    color: #333;
                }
                .header {
                    text-align: center;
                    border-bottom: 2px solid #333;
                    margin-bottom: 20px;
                    padding-bottom: 10px;
                }
                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                }
                th, td {
                    border: 1px solid #333;
                    padding: 8px;
                    text-align: left;
                }
                th {
                    background-color: #f2f2f2;
                }
                .footer {
                    text-align: center;
                    font-size: 12px;
                }
            </style>
        </head>
        <body>
        <div class=""header"">
            <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Logo.png?alt=media&token=e645be03-ac1f-4b28-8aea-4ab2a7c6ea61"" alt=""Company Logo"" style=""height: 50px;"">
            <p style=""margin-top:-5px"">Phone: (123) 456-7890 | Email: info@maincompany.com</p>
        </div>");


                htmlBuilder.Append($@"
            <h4>User Account Detail</h4>
            <table>
                <thead>
                    <tr>
                        
                        <th>User</th>
                        <th>Company</th>
                        <th>Email</th>
                        <th>Access Role</th>
                        <th>App ID</th>
                    </tr>
                </thead>
                <tbody>");

                // Iterate over employees
                foreach (var user in allUsers)
                {
                    htmlBuilder.Append($@"
                    <tr>
                       
                        <td>{user.FirstName} {user.LastName}</td>
                        <td>{user.CompanyName}</td>
                        <td>{user.Email}</td>
                        <td>{user.Roles}</td>
                        <td>{user.EmployeeId}</td>
                    </tr>
                    "
                  );
                }

                htmlBuilder.Append($@"
                   </tbody>
                   </table>
                   <hr />
                ");
                // Close HTML
                htmlBuilder.Append(@"
                   <table style=""width:100%; margin-top: 50px;bordor:none;"">
                   <tr>
                   <td style=""text-align:right;"">
                       <div style=""background-color:#f2f2f2; display:inline-block; padding:10px; border-radius:5px;"">
                           <strong>REMIT TO</strong>
                       </div>
                       <div style=""text-align:right; margin-top:10px;"">
                           Arenas Pass Inc.<br>
                           123 Business St., Suite 456<br>
                           New York, NY 10001<br>
                           USA
                       </div>
                   </td>
                  </tr>
                 </table>
                ");

                response.Response    = htmlBuilder.ToString();
                response.Description = "Report generated successfully";
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message };
            }
        }

        public async Task<ResponseModel<string>> SingleGfitCardReport(string SerialNo)
        {
            try
            {
                // Initialize Response
                var response = new ResponseModel<string> { Status_Code = "200", Description = "OK" };

                // Fetch all employees
                GiftCard Gift = await _giftCardService.FindOne(x=>x.Serial_Number==SerialNo);
                if (Gift==null)
                {
                    return new ResponseModel<string> { Status_Code = "404", Description = "No card Found against serial No "+SerialNo };
                }

                // Start HTML Builder
                var htmlBuilder = new StringBuilder();
                htmlBuilder.Append(@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Gift Card Report</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    margin: 20px;
                    color: #333;
                }
                .header {
                    text-align: center;
                    border-bottom: 2px solid #333;
                    margin-bottom: 20px;
                    padding-bottom: 10px;
                }
                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                }
                th, td {
                    border: 1px solid #333;
                    padding: 8px;
                    text-align: left;
                }
                th {
                    background-color: #f2f2f2;
                }
                .footer {
                    text-align: center;
                    font-size: 12px;
                }
            </style>
        </head>
        <body>
        <div class=""header"">
            <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Logo.png?alt=media&token=e645be03-ac1f-4b28-8aea-4ab2a7c6ea61"" alt=""Company Logo"" style=""height: 50px;"">
            <p style=""margin-top:-5px"">Phone: (123) 456-7890 | Email: info@maincompany.com</p>
        </div>");


                htmlBuilder.Append($@"
               
                <div class=""section-title"">General Information</div>
                <table>
                    <tr>
                        <th>ID</th>
                        <th>Type</th>
                        <th>Tenant ID</th>
                        <th>Organization Name</th>
                    </tr>
                    <tr>
                        <td>{Gift.Id}</td>
                        <td>{Gift.Type}</td>
                        <td>{Gift.TenantId}</td>
                        <td>{Gift.Organization_Name}</td>
                    </tr>
                </table>
                ");


                htmlBuilder.Append($@"
               
                <!-- Pass Information Table -->
                <div class=""section-title"">Pass Information</div>
                <table>
                    <tr>
                        <th>Logo Text</th>
                        <th>Balance</th>
                        <th>Card Holder Name</th>
                        <th>Barcode Type</th>
                    </tr>
                    <tr>
                        <td>{Gift.Logo_Text}</td>
                        <td>{Gift.Currency_Sign}{Gift.Balance}</td>
                        <td>{Gift.Card_holder_Name}</td>
                        <td>{Gift.Code_Type}</td>
                    </tr>
                </table>

                ");

                htmlBuilder.Append($@"
               
                 <!-- Redemption Details Table -->
                <div class=""section-title"">Redemption Details</div>
                <table>
                    <tr>
                        <th>Expiration Date</th>
                        <th>Recipient Name</th>
                        <th>Sender Name</th>
                    </tr>
                    <tr>
                        <td>{Gift.Expiration_Date}</td>
                        <td>{Gift.Recipient_Name}</td>
                        <td>{Gift.Sender_Name}</td>
                    </tr>
                </table>

                ");


                htmlBuilder.Append($@"
               
                  <!-- Contact Information Table -->
                    <div class=""section-title"">Contact Information</div>
                    <table>
                        <tr>
                            <th>Email</th>
                            <th>Phone</th>
                            <th>Address</th>
                        
                        </tr>
                        <tr>
                            <td>{Gift.Email}</td>
                            <td>{Gift.Phone}</td>
                            <td>{Gift.Address}</td>
                           
                        </tr>
                    </table>
                ");


                // Close HTML
                htmlBuilder.Append(@"
                   <table style=""width:100%; margin-top: 50px;bordor:none;"">
                   <tr>
                   <td style=""text-align:right;"">
                       <div style=""background-color:#f2f2f2; display:inline-block; padding:10px; border-radius:5px;"">
                           <strong>REMIT TO</strong>
                       </div>
                       <div style=""text-align:right; margin-top:10px;"">
                           Arenas Pass Inc.<br>
                           123 Business St., Suite 456<br>
                           New York, NY 10001<br>
                           USA
                       </div>
                   </td>
                  </tr>
                 </table>
                ");

                response.Response = htmlBuilder.ToString();
                response.Description = "Report generated successfully";
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message };
            }
        }

        public async Task<ResponseModel<string>> AllWalletPassesReport(List<string> SerialNos)
        {
            try
            {
                // Initialize Response
                var response = new ResponseModel<string> { Status_Code = "200", Description = "OK" };

                // Fetch all Wallet Passes by Serial Numbers
                var passes = await _giftCardService.Find(x => SerialNos.Contains(x.Serial_Number));
                if (passes == null || passes.Count == 0)
                {
                    return new ResponseModel<string> { Status_Code = "404", Description = "No passes found for the provided serial numbers." };
                }

                // Start HTML Builder
                var htmlBuilder = new StringBuilder();
                htmlBuilder.Append(@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Wallet Passes Report</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            margin: 20px;
                            color: #333;
                        }
                        .header {
                            text-align: center;
                            border-bottom: 2px solid #333;
                            margin-bottom: 20px;
                            padding-bottom: 10px;
                        }
                        table {
                            width: 100%;
                            border-collapse: collapse;
                            margin-bottom: 20px;
                        }
                        th, td {
                            border: 1px solid #333;
                            padding: 8px;
                            text-align: left;
                        }
                        th {
                            background-color: #f2f2f2;
                        }
                        .footer {
                            text-align: center;
                            font-size: 12px;
                        }
                    </style>
                </head>
                <body>
                <div class=""header"">
                    <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Logo.png?alt=media&token=e645be03-ac1f-4b28-8aea-4ab2a7c6ea61"" alt=""Company Logo"" style=""height: 50px;"">
                    <p style=""margin-top:-5px"">Phone: (123) 456-7890 | Email: info@maincompany.com</p>
                </div>");

                // Loop through each pass and generate HTML
                foreach (var pass in passes)
                {
                    htmlBuilder.Append($@"
            <div class=""section-title"">General Information for Pass {pass.Serial_Number}</div>
            <table>
                <tr>
                    <th>ID</th>
                    <th>Type</th>
                    <th>Tenant ID</th>
                    <th>Organization Name</th>
                </tr>
                <tr>
                    <td>{pass.Id}</td>
                    <td>{pass.Type}</td>
                    <td>{pass.TenantId}</td>
                    <td>{pass.Organization_Name}</td>
                </tr>
            </table>");

                    // Pass Information Table
                    htmlBuilder.Append($@"
            <div class=""section-title"">Pass Information</div>
            <table>
                <tr>
                    <th>Logo Text</th>
                    <th>Card Holder Name</th>
                    <th>Encoding Type</th>
                </tr>
                <tr>
                    <td>{pass.Logo_Text}</td>
                    <td>{pass.Card_holder_Name}</td>
                    <td>{pass.Code_Type}</td>
                </tr>
            </table>");

                    // Redemption Details Table
                    htmlBuilder.Append($@"
            <div class=""section-title"">Redemption Details</div>
            <table>
                <tr>
                    <th>Expiration Date</th>
                    <th>Recipient Name</th>
                    <th>Sender Name</th>
                </tr>
                <tr>
                    <td>{pass.Expiration_Date}</td>
                    <td>{pass.Recipient_Name}</td>
                    <td>{pass.Sender_Name}</td>
                </tr>
            </table>");

                    // Contact Information Table
                    htmlBuilder.Append($@"
            <div class=""section-title"">Contact Information</div>
            <table>
                <tr>
                    <th>Email</th>
                    <th>Phone</th>
                    <th>Address</th>
                </tr>
                <tr>
                    <td>{pass.Email}</td>
                    <td>{pass.Phone}</td>
                    <td>{pass.Address}</td>
                </tr>
            </table>");
                }

                // Close HTML
                htmlBuilder.Append(@"
        <table style=""width:100%; margin-top: 50px;border:none;"">
        <tr>
        <td style=""text-align:right;"">
            <div style=""background-color:#f2f2f2; display:inline-block; padding:10px; border-radius:5px;"">
                <strong>REMIT TO</strong>
            </div>
            <div style=""text-align:right; margin-top:10px;"">
                Arenas Pass Inc.<br>
                123 Business St., Suite 456<br>
                New York, NY 10001<br>
                USA
            </div>
        </td>
        </tr>
        </table>");

                // Complete Response
                response.Response = htmlBuilder.ToString();
                response.Description = "Report generated successfully for all passes";
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message };
            }
        }

    }
}
