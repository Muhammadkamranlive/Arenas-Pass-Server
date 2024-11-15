using Server.Models;
using Server.Domain;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.DigitalPasses.ApplePassAccount
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplePassController : ControllerBase
    {
        private readonly IApple_Pass_Account_Service applePassAccount_Service;
        private readonly IHttpContextAccessor        _httpContextAccessor;
        public ApplePassController
        (
          IApple_Pass_Account_Service apaService,
          IHttpContextAccessor        httpContextAccessor
        )
        {
            applePassAccount_Service = apaService;
            _httpContextAccessor     = httpContextAccessor;

        }

        [HttpGet]
        [Route("GetApplePassAccount")]
        [CustomAuthorize("Read")]
        public async Task<IList<Apple_Pass_Account>> GetApplePassAccount()
        {
            try
            {
                var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
                return await applePassAccount_Service.Find(x => x.TenantId == tenantId);
            }
            catch (Exception e)
            {
              throw new Exception(e.Message + e.InnerException?.Message);
            }
        }
    }
}
