using Server.Models;
using Server.Domain;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.licenseKeyController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseKeyController : ControllerBase
    {
        private readonly ITenant_License_Keys_Service tlk_Service;
        public LicenseKeyController
        (
               ITenant_License_Keys_Service tenantService
        )
        {
            tlk_Service=tenantService;
        }


        [HttpGet]
        [Route("CreateOrUpdateKeysForTenant")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> CreateOrUpdateKeysForTenant()
        {
            try
            {

                TenantLicenes obj   = await tlk_Service.CreateOrUpdateKeysForTenant();

                return Ok(obj);

            }
            catch (Exception ex)
            {
                var successResponse = new SuccessResponse
                {
                    Message = "Error" + ex.Message + (string.IsNullOrEmpty(ex.InnerException.Message) ? "" : ex.InnerException.Message)
                };
                return BadRequest(successResponse);
            }

        }


        [HttpGet]
        [Route("GetAPIKeysForTenant")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetAPIKeysForTenant()
        {
            try
            {

                TenantLicenes obj = await tlk_Service.GetApiKeys();

                return Ok(obj);

            }
            catch (Exception ex)
            {
                var successResponse = new SuccessResponse
                {
                    Message = "Error" + ex.Message + (string.IsNullOrEmpty(ex.InnerException.Message) ? "" : ex.InnerException.Message)
                };
                return BadRequest(successResponse);
            }
        }

    }
}
