using Server.Services;
using Server.Repository;
using Server.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.DigitalPasses.WalletPass
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletPassController : ControllerBase
    {
        private readonly IWallet_Pass_Service   _Pass_Service;
        private readonly IGet_Tenant_Id_Service tenant_Id_Service;

        public WalletPassController
        (
          IWallet_Pass_Service _Pass,
          IGet_Tenant_Id_Service Tenant_Id
        )
        {
            _Pass_Service     = _Pass;
            tenant_Id_Service = Tenant_Id;
        }



        [HttpGet]
        [Route("GetWalletPasses")]
        public async Task<dynamic> GetWalletPasses()
        {
            try
            {
                var TenantId = tenant_Id_Service.GetTenantId();
                var Apis     = await _Pass_Service.Find(x => x.TenantId == TenantId && x.Pass_Status==Pass_Redemption_Status_GModel.Template);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetWallet")]
        public async Task<dynamic> GetWallet()
        {
            try
            {
                
                var Apis = _Pass_Service.GetAllByParent();
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
