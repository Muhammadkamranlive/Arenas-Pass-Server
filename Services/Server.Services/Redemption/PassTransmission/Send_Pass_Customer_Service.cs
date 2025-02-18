using Azure;
using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using Server.Configurations;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Server.Domain.PassTransmission;
using Microsoft.EntityFrameworkCore.Storage;

namespace Server.Services
{
    public class Send_Pass_Customer_Service:Base_Service<Pass_Transmission>, ISend_Pass_Customer_Service
    {
        #region Constructor
        private readonly IWallet_Pass_Service         _wallet_Pass_Service;
        private readonly IEmail_Service               _email_Service;
        private readonly IAccount_Balance_Service     _account_Balance_Service;
        private readonly IGet_Tenant_Id_Service       _Tenant_Id_Service;
        private readonly ERPDb                       _context;
        public Send_Pass_Customer_Service
        (   IUnit_Of_Work_Repo                   unitOfWork,
            IPass_Transmission_Repo       genericRepository,
            IWallet_Pass_Service          wallet_Pass,
            IEmail_Service                email,
            IAccount_Balance_Service      account_Balance,
            IGet_Tenant_Id_Service        tenant,
            ERPDb                      context


        ) : base(unitOfWork, genericRepository)
        {
           
            _wallet_Pass_Service     = wallet_Pass;
            _email_Service           = email;
            _account_Balance_Service = account_Balance;
            _Tenant_Id_Service       = tenant;
            _context                 = context;
        }

        public async Task<ResponseModel<string>> SendtoUser(IList<string> listCards,string message,string subjectmessage)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>() { Status_Code = "200",Description="OK",Response="OK" };

                //find cards list
                var tenantId = _Tenant_Id_Service.GetTenantId();

                IList<WalletPass> passesList   = await _wallet_Pass_Service.Find(x => listCards.Contains(x.Serial_Number) && x.TenantId==tenantId);
                response                       = CatchExceptionNull(passesList);
                if (response.Status_Code != "200")
                {
                    return response;
                }
                
                IList<Pass_Transmission> list = new List<Pass_Transmission>();
                for (int i = 0; i < passesList.Count; i++)
                {
                    string otp       = passesList[i].Serial_Number;
                    string username  = passesList[i].Recipient_Name;
                    string body      = CreateEmailBodyCustomMessage(otp, username,passesList[i].Address, passesList[i].Sender_Name,message);
                    string subject   = subjectmessage;
                    //loop through email
                    await _email_Service.SendEmailAsync(passesList[i].Email,subject,body);
                    Pass_Transmission model = new Pass_Transmission()
                    {
                        Pass_Trans_Status = Pass_Transmission_Status_GModel.Sent,
                        Card_Id           = passesList[i].Id,
                        Email             = passesList[i].Email
                    };
                    passesList[i].Pass_Status = Pass_Redemption_Status_GModel.Redeemable;
                    list.Add(model);
                }
                _wallet_Pass_Service.Update(passesList,x=>x.Pass_Status);

                await AddRange(list);
                await CompleteAync();
                return response;
            }
            catch (Exception ex)
            {

                return CatchException(ex);
            }
        }
        public async Task<ResponseModel<string>> SendtoUser(IList<string> listCards)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>() { Status_Code = "200",Description="OK",Response="OK" };

                //find cards list
                var tenantId = _Tenant_Id_Service.GetTenantId();

                IList<WalletPass> passesList   = await _wallet_Pass_Service.Find(x => listCards.Contains(x.Serial_Number) && x.TenantId==tenantId);
                response                       = CatchExceptionNull(passesList);
                if (response.Status_Code != "200")
                {
                    return response;
                }
                
                IList<Pass_Transmission> list = new List<Pass_Transmission>();
                for (int i = 0; i < passesList.Count; i++)
                {
                    string otp       = passesList[i].Serial_Number;
                    string username  = passesList[i].Recipient_Name;
                    string body      = CreateEmailBody(otp, username,passesList[i].Address, passesList[i].Sender_Name);
                    string subject   = $@"Congratulations! You've Received a {passesList[i].Type} from {passesList[i].Sender_Name}";
                    //loop through email
                    await _email_Service.SendEmailAsync(passesList[i].Email,subject,body);
                    Pass_Transmission model = new Pass_Transmission()
                    {
                        Pass_Trans_Status = Pass_Transmission_Status_GModel.Sent,
                        Card_Id           = passesList[i].Id,
                        Email             = passesList[i].Email
                    };
                    passesList[i].Pass_Status = Pass_Redemption_Status_GModel.Redeemable;
                    list.Add(model);
                    
                }
                
                _wallet_Pass_Service.Update(passesList,x=>x.Pass_Status);
                
                await AddRange(list);
                await CompleteAync();
                return response;
            }
            catch (Exception ex)
            {

                return CatchException(ex);
            }
        }
        #endregion





        private static string CreateEmailBody(string OTP, string Username,string storeLocation,string StoreName)
        {
            string emailTemplate = $@"
               <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Template</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
            color: #333;
        }}
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border: 1px solid #ddd;
            border-radius: 8px;
            overflow: hidden;
        }}
        .header {{
            background-color: #007bff;
            color: white;
            text-align: center;
            padding: 20px;
        }}
        .header img {{
            max-width: 150px;
            margin-bottom: 10px;
        }}
        .content {{
            padding: 20px;
            line-height: 1.6;
        }}
        .table-container {{
            margin-top: 20px;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin: 10px 0;
        }}
        th, td {{
            padding: 10px;
            text-align: left;
            border: 1px solid #ddd;
        }}
        th {{
            background-color: #f8f8f8;
        }}
        .footer {{
            background-color: #007bff;
            color: white;
            text-align: center;
            padding: 20px;
            font-size: 12px;
        }}
        .footer a {{
            color: #ffdd57;
            text-decoration: none;
        }}
        .footer a:hover {{
            text-decoration: underline;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/icon-128x128.png?alt=media&token=1b17b3bf-2bd7-441a-a16f-e747420c4783"" alt=""Arenas Pass Logo"">
            <h1>Welcome to Arenas Pass</h1>
            <p>Your gateway to exclusive offers and deals!</p>
        </div>

        <div class=""content"">
            <h2>Dear {Username},</h2>
            <p>We’re excited to inform you that you’ve received a pass from <strong>{StoreName}</strong>! 🎉</p>

            <h3 style=""color: black;"">Pass Details:</h3>
            <div class=""table-container"">
                <table>
                    <tr>
                        <th>From</th>
                        <td>{StoreName}</td>
                    </tr>
                    <tr>
                        <th>Redeem at</th>
                        <td>{storeLocation}</td>
                    </tr>
                    <tr>
                        <th>Pass Serial Number</th>
                        <td>{OTP}</td>
                    </tr>
                </table>
            </div>

            <p style=""color: black;"">To redeem your pass, simply visit the store at the above address.</p>

            <strong style=""color: black;"">Important: Please keep your pass's serial number confidential and do not share it with anyone to ensure its security.</strong>

            <p style=""color: black;"">If you have any questions or need assistance, feel free to reach out to us.</p>

            <p style=""color: black;"">Thank you for choosing <strong>{StoreName}</strong>!</p>
        </div>

        <div class=""footer"">
            <p>Need Help? Please send any feedback or bug info to <a href=""mailto:info@arenaspass.com"">info@arenaspass.com</a></p>
            <p>950 Dannon View, SW Suite 4103 Atlanta, GA 30331</p>
            <p>Phone: <a href=""tel:4045930993"">404-593-0993</a></p>
        </div>
    </div>
</body>
</html>

            ";
            return emailTemplate;
        }

         private static string CreateEmailBodyCustomMessage(string OTP, string Username,string storeLocation,string StoreName,string message)
        {
            string emailTemplate = $@"
               <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Template</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
            color: #333;
        }}
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border: 1px solid #ddd;
            border-radius: 8px;
            overflow: hidden;
        }}
        .header {{
            background-color: #007bff;
            color: white;
            text-align: center;
            padding: 20px;
        }}
        .header img {{
            max-width: 150px;
            margin-bottom: 10px;
        }}
        .content {{
            padding: 20px;
            line-height: 1.6;
        }}
        .table-container {{
            margin-top: 20px;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin: 10px 0;
        }}
        th, td {{
            padding: 10px;
            text-align: left;
            border: 1px solid #ddd;
        }}
        th {{
            background-color: #f8f8f8;
        }}
        .footer {{
            background-color: #007bff;
            color: white;
            text-align: center;
            padding: 20px;
            font-size: 12px;
        }}
        .footer a {{
            color: #ffdd57;
            text-decoration: none;
        }}
        .footer a:hover {{
            text-decoration: underline;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <img src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/icon-128x128.png?alt=media&token=1b17b3bf-2bd7-441a-a16f-e747420c4783"" alt=""Arenas Pass Logo"">
            <h1>Welcome to Arenas Pass</h1>
            <p>Your gateway to exclusive offers and deals!</p>
        </div>

        <div class=""content"">
            <h2>Dear {Username},</h2>
            <p>
              {message} 
           </p>

            <h3 style=""color: black;"">Pass Details:</h3>
            <div class=""table-container"">
                <table>
                    <tr>
                        <th>From</th>
                        <td>{StoreName}</td>
                    </tr>
                    <tr>
                        <th>Redeem at</th>
                        <td>{storeLocation}</td>
                    </tr>
                    <tr>
                        <th>Pass Serial Number</th>
                        <td>{OTP}</td>
                    </tr>
                </table>
            </div>

            <p style=""color: black;"">To redeem your pass, simply visit the store at the above address.</p>

            <strong style=""color: black;"">Important: Please keep your pass's serial number confidential and do not share it with anyone to ensure its security.</strong>

            <p style=""color: black;"">If you have any questions or need assistance, feel free to reach out to us.</p>

            <p style=""color: black;"">Thank you for choosing <strong>{StoreName}</strong>!</p>
        </div>

        <div class=""footer"">
            <p>Need Help? Please send any feedback or bug info to <a href=""mailto:info@arenaspass.com"">info@arenaspass.com</a></p>
            <p>950 Dannon View, SW Suite 4103 Atlanta, GA 30331</p>
            <p>Phone: <a href=""tel:4045930993"">404-593-0993</a></p>
        </div>
    </div>
</body>
</html>

            ";
            return emailTemplate;
        }

        public async Task<IList<WalletPass>> GetWalletPasses()
        {
            try
            {
                var tenantId                   = _Tenant_Id_Service.GetTenantId();
               
                IList<WalletPass> passesList1    =  await _context.WalletPasses.Where(x=>x.Pass_Status==Pass_Redemption_Status_GModel.PendingForSend)
                   .ToListAsync();  
                return passesList1;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
