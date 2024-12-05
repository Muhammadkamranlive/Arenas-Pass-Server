using Azure;
using Server.Models;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.BulkImport
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkImportController : ControllerBase
    { 
        private readonly IExcell_Import_Service     ExService;
        private readonly IGet_Tenant_Id_Service     TenantIdService;
        public BulkImportController
        (
            IExcell_Import_Service    exService,
            IGet_Tenant_Id_Service    get_Tenant_Id
        )
        {
            ExService       = exService;
            TenantIdService = get_Tenant_Id;
        }


        [HttpPost("UploadExcel")]
        [CustomAuthorize("Write")]
        public async Task<dynamic> UploadExcelFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid Excel file.");
            var successResponse = new SuccessResponse();
            try
            {
               
                ResponseModel<string> response = await ExService.UploadCustomerBatchFile(file);
                if (response.Status_Code == "200")
                {
                    successResponse.Message = response.Description;
                    return Ok(successResponse);
                }
                else
                {
                    successResponse.Message = response.Description;
                    return BadRequest(successResponse);
                }
            }
            catch (Exception ex)
            {
                successResponse.Message = $"Error processing file: {ex.Message}";
                return BadRequest(successResponse);
            }
        }

        [HttpPost("UploadUsersFile")]
        [CustomAuthorize("Write")]
        public async Task<dynamic> UploadUsersFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid Excel file.");
            var successResponse = new SuccessResponse();
            try
            {

                ResponseModel<string> response = await ExService.UploadCustomerBatchFile(file);
                if (response.Status_Code == "200")
                {
                    successResponse.Message = response.Description;
                    return Ok(successResponse);
                }
                else
                {
                    successResponse.Message = response.Description;
                    return BadRequest(successResponse);
                }
            }
            catch (Exception ex)
            {
                successResponse.Message = $"Error processing file: {ex.Message}";
                return BadRequest(successResponse);
            }
        }


        [HttpGet("GetPendingBatchProcess")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetPendingBatchProcess()
        {
            try
            {
                var tenantId = TenantIdService.GetTenantId();
                var list     = await ExService.TenantUnProcessedBatchWithProp(tenantId,"Customer");
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("PeformBatchProcess")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> PeformBatchProcess()
        {
            try
            {
                var successResponse = new SuccessResponse();

                var response     = await ExService.ProcessUploadedCustomerFile();
                if (response.Status_Code == "200")
                {
                    successResponse.Message = response.Description;
                    return Ok(successResponse);
                }
                else
                {
                    successResponse.Message = response.Description;
                    return BadRequest(successResponse);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
