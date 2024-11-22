using AutoMapper;
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
        private readonly IApple_Pass_Account_Service _applePassAccount_Service;
        private readonly IHttpContextAccessor        _httpContextAccessor;
        private readonly IMapper                     _iMapper;
        public ApplePassController
        (
          IApple_Pass_Account_Service apaService,
          IHttpContextAccessor        httpContextAccessor,
          IMapper                     Mapper
        )
        {
            _applePassAccount_Service =  apaService;
            _httpContextAccessor      =  httpContextAccessor;
            _iMapper                  =  Mapper;

        }

        [HttpGet]
        [Route("GetApplePassAccount")]
        [CustomAuthorize("Read")]
        public async Task<IList<Apple_Pass_Account>> GetApplePassAccount()
        {
            try
            {
                var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
                return await _applePassAccount_Service.Find(x => x.TenantId == tenantId);
            }
            catch (Exception e)
            {
              throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpPost]
        [Route("EditRequestApplePassAccount")]
        [CustomAuthorize("Edit")]
        public async Task<string> EditRequestApplePassAccount(ApplePassAccountModel Model)
        {
            try
            {
                var model = _iMapper.Map<Apple_Pass_Account>(Model);
                _applePassAccount_Service.UpdateRecord(model);
                await _applePassAccount_Service.CompleteAync();
                return "OK";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }



        [HttpPost]
        [Route("NewApplePassAccount")]
        [CustomAuthorize("Add")]
        public async Task<string> NewApplePassAccount(ApplePassAccountModel Model)
        {
            try
            {
                var model = _iMapper.Map<Apple_Pass_Account>(Model);
                _applePassAccount_Service.UpdateRecord(model);
                await _applePassAccount_Service.CompleteAync();
                return "OK";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }
    }
}
