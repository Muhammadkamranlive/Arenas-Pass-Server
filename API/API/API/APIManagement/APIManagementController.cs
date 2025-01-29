using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.APIManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIManagementController : ControllerBase
    {
        private readonly ITenant_Api_Hits_Service     tenant_Api_Hits_Service;
        private readonly IGet_Tenant_Id_Service       tenant_Id_Service;
        private readonly ITenant_License_Keys_Service TenantLicenService;
        private readonly ITenant_Key_History_Service  tkh_Service;
        private readonly ITenants_Service             tenants_Service;
        public APIManagementController
        (
             ITenant_Api_Hits_Service     tenant_Api_Hits,
             IGet_Tenant_Id_Service       get_Tenant_Id,
             ITenant_License_Keys_Service tenant_License,
             ITenant_Key_History_Service  history_Service,
             ITenants_Service             tenants
        )
        {
            tenant_Api_Hits_Service = tenant_Api_Hits;
            tenant_Id_Service       = get_Tenant_Id;
            TenantLicenService      = tenant_License;
            tkh_Service             = history_Service;
            tenants_Service         = tenants;
        }

        [HttpGet]
        [Route("GetApiStats")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetApiStats()
        {
            try
            {
                 var TenantId  = tenant_Id_Service.GetTenantId();
                 var Apis      = await tenant_Api_Hits_Service.Find(x => x.TenantId == TenantId);
                 return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetTenantApiKeys")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetTenantApiKeys()
        {
            try
            {
                var TenantId = tenant_Id_Service.GetTenantId();
                var Apis     = await TenantLicenService.Find(x => x.TenantId == TenantId);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetTenantDetail")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetTenantDetail()
        {
            try
            {
                var TenantId = tenant_Id_Service.GetTenantId();
                var Apis     = await tenants_Service.FindOne(x => x.CompanyId == TenantId);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [HttpGet]
        [Route("GetTenantKeyRotationHistory")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetTenantKeyRotationHistory()
        {
            try
            {
                var TenantId = tenant_Id_Service.GetTenantId();
                var Apis     = await tkh_Service.Find(x => x.TenantId == TenantId);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
