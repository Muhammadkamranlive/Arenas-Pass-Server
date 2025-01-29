using Server.Domain;
using Server.Models;
using Server.Services;
using Server.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace API.API.DigitalPasses.LoyaltyCards
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoayaltyController : ControllerBase
    {
        #region Contructor
        private readonly ILoyalty_Card_Service      _memer_Service;
        private readonly IGet_Tenant_Id_Service     _httpContextAccessor;
        public LoayaltyController
        (
          ILoyalty_Card_Service gcards_Service, IGet_Tenant_Id_Service httpContext
        )
        {
            _memer_Service = gcards_Service; _httpContextAccessor = httpContext;
        }
        #endregion

        [HttpPost]
        [Route("GenerateCard")]
        public async Task<dynamic> GenerateCard(Apple_Passes_Loyalty_Card_Model model)
        {
            try
            {

                ResponseModel<string> res = await _memer_Service.GenerateCard(model);
                if (res.Status_Code != "200")
                {
                    return res;
                }
                var successResponse = new SuccessResponse
                {
                    Message = "Your Loyalty Card Template is saved successfully"
                };
                return Ok(successResponse);
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
        [Route("GetMerchantsCard")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetMerchantsCard()
        {
            try
            {

                int tenantId                   = _httpContextAccessor.GetTenantId();
                IEnumerable<LoyaltyCard> gifts = await _memer_Service.Find(x => x.TenantId == tenantId && x.Pass_Status == Pass_Redemption_Status_GModel.Template);
                return gifts;

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
        [Route("GetMerchantsRedeemableCard")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetMerchantsRedeemableCard()
        {
            try
            {

                int tenantId                   = _httpContextAccessor.GetTenantId();
                IEnumerable<LoyaltyCard> gifts = await _memer_Service.Find(x => x.TenantId == tenantId && x.Pass_Status != Pass_Redemption_Status_GModel.Template);
                return gifts;

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
        [Route("GetMerchantsCardById")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetMerchantsCardById(int Id)
        {
            try
            {

                int tenantId = _httpContextAccessor.GetTenantId();
                var gifts    = await _memer_Service.FindOne(x => x.Id == Id && x.TenantId == tenantId);
                return gifts;

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
        [Route("GetCardById")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetCardById(int Id)
        {
            try
            {

                int tenantId = _httpContextAccessor.GetTenantId();
                var gifts    = await _memer_Service.FindOne(x => x.Id == Id && x.TenantId == tenantId);

                return gifts;

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



        [HttpPut]
        [Route("UpdateMerchantsCard")]
        [CustomAuthorize("Update")]
        public async Task<dynamic> UpdateMerchantsCard(Apple_Passes_Loyalty_Card_Model model)
        {
            try
            {

                var res = await _memer_Service.UpdateGiftCard(model);
                if (res.Status_Code != "200")
                {
                    var successRespons = new SuccessResponse
                    {
                        Message = res.Description
                    };
                    return BadRequest(successRespons);
                }
                var successResponse = new SuccessResponse
                {
                    Message = "Your Loyalty Card Template is updated successfully"
                };
                return Ok(successResponse);

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

        [HttpDelete]
        [Route("DeleteMerchantsCard")]
        [CustomAuthorize("Delete")]
        public async Task<dynamic> DeleteMerchantsCard(int Id)
        {
            try
            {

                int tenantId = _httpContextAccessor.GetTenantId();
                var res = await _memer_Service.DeleteGiftCard(Id, tenantId);
                if (res.Status_Code == "200")
                {
                    await _memer_Service.CompleteAync();
                    var successResponse = new SuccessResponse
                    {
                        Message = "Loyalty Card Tempate Deleted Successfully"
                    };

                    return Ok(successResponse);
                }
                else
                {
                    var successResponse = new SuccessResponse
                    {
                        Message = "Error in Deletion"
                    };
                    return BadRequest(successResponse);
                }

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

        [HttpPost]
        [Route("DownloadCard")]
        public async Task<dynamic> DownloadCard(Apple_Passes_Loyalty_Card_Model GiftCard)
        {
            try
            {

                ResponseModel<string> res = await _memer_Service.GenerateCard(GiftCard);
                if (res.Status_Code != "200")
                {
                    return res;
                }
                byte[] generatedPass = (byte[])res.Response;
                return new FileContentResult(generatedPass, "application/vnd.apple.pkpass")
                {
                    FileDownloadName = "File123" + ".pkpass"
                };
            }
            catch (Exception ex)
            {
                ResponseModel<string> giftResponse = new ResponseModel<string>()
                {
                    Status_Code = "500",
                    Description = ex.Message,
                    Response = "Error Occurred"
                };
                return giftResponse;
            }
        }

    }
}
