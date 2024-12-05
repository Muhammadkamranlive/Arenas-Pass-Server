using System;
using System.Linq;
using System.Text;
using Server.Models;
using Server.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IExcell_Import_Service
    {
        Task<ResponseModel<string>> ProcessUploadedCustomerFile();
        Task<ResponseModel<string>> UploadCustomerBatchFile(IFormFile file);
        Task<IList<UserVoucher>> TenantUnProcessedBatchWithProp(int tenantId,string UserType);
        Task<IList<UserVoucher>> TenantUnProcessedBatch(int tenantId);
        Task<ResponseModel<string>> ProcessUploadedUserFile(string Role);
    }
}
