using Server.Domain;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBillingAddressController : ControllerBase
    {
        private readonly IUserBilling_Address_Service _billing_Address_Service;
        public UserBillingAddressController(IUserBilling_Address_Service address_Service)
        {
             _billing_Address_Service = address_Service;
        }

        [HttpPost]
        [Route("AddOrUpdateBillingAddress")]
        public async Task<dynamic> AddOrUpdateBillingAddress( UserBillingDetail model)
        {
            try
            {
                  var res  = await _billing_Address_Service.AddOrUpdateBilingDetail(model);
                  return Ok(res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("FindBillingBillingAddress")]
        public async Task<dynamic> FindBillingBillingAddress(string email)
        {
            try
            {
                var res  = await _billing_Address_Service.FindOne(x=>x.Email == email);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
