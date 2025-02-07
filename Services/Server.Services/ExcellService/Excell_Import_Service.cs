using System;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using Server.Models;
using Server.Domain;
using Server.BaseService;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class Excell_Import_Service: IExcell_Import_Service
    {
        private readonly IBulk_User_Import_Service   blkService;
        private readonly IGet_Tenant_Id_Service      getTenantIdService;
        private readonly IAuth_Manager_Service                authManagerService;
        private readonly ITransaction_No_Service     empIdService;
        private readonly IUser_Batch_Process_Service btchService;
        public Excell_Import_Service
        (
            IBulk_User_Import_Service   blk, 
            IGet_Tenant_Id_Service      getTenantId, 
            IAuth_Manager_Service                authManager,
            ITransaction_No_Service     empId,
            IUser_Batch_Process_Service btch
        )
        {
            blkService         = blk;
            getTenantIdService = getTenantId;
            authManagerService = authManager;
            empIdService       = empId;
            btchService        = btch;
        }



        public async Task<ResponseModel<string>> ProcessUploadedUserFile(string Role)
        {
            var users    = new List<ApplicationUser>();
            var tenantId = getTenantIdService.GetTenantId();
            var UserId   = getTenantIdService.GetUserId();
            try
            {
                ResponseModel<string> retValue = new ();
                var BatchFiles                 = await TenantUnProcessedBatchWithProp(tenantId,"Customer");
                if (BatchFiles.Count == 0)
                {

                    retValue.Status_Code  = "400";
                    retValue.Description = "There is no batch file to process Please Upload Batch File first";
                    
                    return retValue;
                }
                ApplicationUser userDetail = await authManagerService.FindById(UserId);
                for (int i = 0; i < BatchFiles.Count; i++)
                {
                    var user = new ApplicationUser
                    {
                        Email           = BatchFiles[i].Email,
                        FirstName       = BatchFiles[i].FirstName,
                        LastName        = BatchFiles[i].LastName,
                        UserName        = BatchFiles[i].Email,
                        isEmployee      = true,
                        isAdmin         = false,
                        defaultPassword = "Asdf@1234",
                        TenantId        = tenantId,
                        CompanyName     = userDetail.CompanyName,
                        image           = "https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/images.jfif?alt=media&token=09284390-5fd1-40f7-b91c-feabadf143a9",
                        LoginRestEnable = true,
                        EmployeeId      = await empIdService.GetTxnNo()
                    };
                    BatchFiles[i].ProcessedByUsername = userDetail.FirstName + " " + userDetail.LastName;
                    BatchFiles[i].Status              = "Processed";
                    BatchFiles[i].ProcessedByUserId   = UserId;
                    users.Add(user);
                }

                if (users.Count > 0)
                {
                   retValue = await blkService.BulkInsertUsersAsync(users,Role);
                   return retValue; 
                }

                
                return retValue;
            }
            catch (Exception ex)
            {
                // Log error for debugging
                Console.WriteLine($"Error processing Excel file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Process Customer Batch File
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<string>> ProcessUploadedCustomerFile()
        {
            var users    = new List<ApplicationUser>();
            var tenantId = getTenantIdService.GetTenantId();
            var UserId   = getTenantIdService.GetUserId();
            try
            {
                ResponseModel<string> retValue = new ();
                var BatchFiles = await TenantUnProcessedBatchWithProp(tenantId,"Customer");
                if (BatchFiles.Count == 0)
                {

                    retValue.Status_Code  = "400";
                    retValue.Description = "There is no batch file to process Please Upload Batch File first";
                    
                    return retValue;
                }
                ApplicationUser userDetail = await authManagerService.FindById(UserId);
                for (int i = 0; i < BatchFiles.Count; i++)
                {
                    var user = new ApplicationUser
                    {
                        Email           = BatchFiles[i].Email,
                        FirstName       = BatchFiles[i].FirstName,
                        LastName        = BatchFiles[i].LastName,
                        UserName        = BatchFiles[i].Email,
                        isEmployee      = true,
                        isAdmin         = false,
                        defaultPassword = "Asdf@1234",
                        TenantId        = tenantId,
                        CompanyName     = userDetail.CompanyName,
                        image           = "https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/images.jfif?alt=media&token=09284390-5fd1-40f7-b91c-feabadf143a9",
                        LoginRestEnable = true,
                        EmployeeId      = await empIdService.GetTxnNo()
                    };
                    BatchFiles[i].ProcessedByUsername = userDetail.FirstName + " " + userDetail.LastName;
                    BatchFiles[i].Status              = "Processed";
                    BatchFiles[i].ProcessedByUserId   = UserId;
                    users.Add(user);
                }



                if (users.Count > 0)
                {
                   retValue = await blkService.BulkInsertUsersAsync(users,"Customer");
                   if (retValue.Status_Code == "200")
                   {
                       btchService.RemoveRange(BatchFiles);
                       await btchService.CompleteAync();
                   }
                   else
                   {

                       btchService.RemoveRange(BatchFiles);
                       await btchService.CompleteAync();
                   }
                   return retValue; 
                }

                
                return retValue;
            }
            catch (Exception ex)
            {
                // Log error for debugging
                Console.WriteLine($"Error processing Excel file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Customer Batch File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseModel<string>> UploadCustomerBatchFile(IFormFile file)
        {
            var users    = new List<UserVoucher>();
            var tenantId = getTenantIdService.GetTenantId();
            var UserId   = getTenantIdService.GetUserId();
            var BatchNo  = tenantId + UserId + DateTime.Now.ToString("hhmmss");
            ResponseModel<string> retValue = new() { Status_Code = "400", Description = "", Response = "OK" };
            try
            {
                if (file == null || file.Length == 0)
                {
                     retValue.Description = "Invalid or empty file";
                     return retValue;
                }
                var BatchFiles              = await TenantUnProcessedBatchWithProp(tenantId);
                if (BatchFiles.Count != 0)
                {
                    retValue.Description    = "Your previouse Batch file is still pending please process it first than Upload Batch";
                    return retValue;
                }
                ApplicationUser userDetail  = await authManagerService.FindById(UserId);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var package = new ExcelPackage(stream))
                    {
                        foreach (var worksheet in package.Workbook.Worksheets)
                        {
                            if (worksheet == null)
                            {
                                continue; 
                            }
                            // Validate headers
                            var headerFirstName = worksheet.Cells[1, 1]?.Text.Trim();
                            var headerLastName  = worksheet.Cells[1, 2]?.Text.Trim();
                            var headerEmail     = worksheet.Cells[1, 3]?.Text.Trim();

                            if (headerFirstName != "First Name" || headerLastName != "Last Name" || headerEmail != "Email")
                            {
                                retValue.Description = "Invalid file format. Ensure the headers are 'First Name', 'Last Name', and 'Email'.";
                                return retValue;
                            }
                            int totalRows = worksheet.Dimension.Rows;

                            for (int row = 2; row <= totalRows; row++) 
                            {
                                var email     = worksheet.Cells[row, 3]?.Text.Trim();
                                var firstName = worksheet.Cells[row, 1]?.Text.Trim();
                                var lastName  = worksheet.Cells[row, 2]?.Text.Trim();

                                // Skip rows with missing required fields
                                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                                {
                                    continue;
                                }
                                if (users.FindIndex(u => u.Email.ToLower() == email.ToLower())>=0)
                                {
                                    var user1                  = new UserVoucher
                                    {
                                        Email                 = email,
                                        FirstName             = firstName,
                                        LastName              = lastName,
                                        Status                = "Error",
                                        Remarks               = "User With Duplicate Email Id in Same Batch",
                                        TenantId              = tenantId,
                                        UploadedByUserId      = UserId,
                                        UploadedByUsername    = userDetail.FirstName +" " +userDetail.LastName,
                                        BatchNo               = BatchNo,
                                        ProcessedByUserId     = null,
                                        ProcessedByUsername   = null,
                                        UserType              = "Customer"
                                    };

                                    users.Add(user1);
                                    continue;
                                }
                                var user                  = new UserVoucher
                                {
                                    Email                 = email,
                                    FirstName             = firstName,
                                    LastName              = lastName,
                                    Status                = "Pending",
                                    Remarks               = "User saved in bucket  and account creation pending until you run Batch Process",
                                    TenantId              = tenantId,
                                    UploadedByUserId      = UserId,
                                    UploadedByUsername    = userDetail.FirstName +" " +userDetail.LastName,
                                    BatchNo               = BatchNo,
                                    ProcessedByUserId     = null,
                                    ProcessedByUsername   = null,
                                    UserType              = "Customer"
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
                
                if (users.Count > 0)
                {
                    var existingEmails = await btchService.Find(u => users.Select(nu => nu.Email).Contains(u.Email));
                    var emails         = existingEmails.Select(x=>x.Email);
                    var uniqueUsers    = users.Where(u => !emails.Contains(u.Email.ToLower())).ToList();
                    if (uniqueUsers.Count> 0)
                    {
                        await btchService.AddRange(uniqueUsers);
                        await btchService.CompleteAync();
                        retValue.Status_Code = "200";
                        retValue.Description = "OK Batch File Uploaded Successfully";
                    }
                    else
                    {
                        retValue.Status_Code = "404";
                        retValue.Description = "Users all users already present in batch process please process batch.";
                    }
                    return retValue; 
                }
                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<UserVoucher>> TenantUnProcessedBatchWithProp(int tenantId)
        {
            try
            {
                var prevFile = await btchService.Find(x => x.Status == "Pending" && x.TenantId==tenantId);
                return prevFile;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<UserVoucher>> TenantUnProcessedBatchWithProp(int tenantId, string UserType)
        {
            try
            {
                var prevFile = await btchService.Find(x => x.Status == "Pending" && x.TenantId == tenantId && x.UserType==UserType);
                return prevFile;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<UserVoucher>> TenantUnProcessedBatch(int tenantId)
        {
            try
            {
                var prevFile = await btchService.Find(x => x.Status == "Pending" && x.TenantId == tenantId);
                return prevFile;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
