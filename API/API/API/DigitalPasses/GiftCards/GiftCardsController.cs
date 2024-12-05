using Server.Models;
using Server.Domain;
using Server.Services;
using Passbook.Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Passbook.Generator.Fields;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace API.API.DigitalPasses
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardsController : ControllerBase
    {
        #region Contructor
        private readonly IGift_Card_Service     _Gift_Service;
        private readonly IGet_Tenant_Id_Service _httpContextAccessor;
        public GiftCardsController
        (
          IGift_Card_Service gcards_Service, IGet_Tenant_Id_Service httpContext    
        )
        {
            _Gift_Service = gcards_Service; _httpContextAccessor = httpContext;
        }
        #endregion

        [HttpPost]
        [Route("GenerateGiftCard")]
        public async Task<dynamic> GenerateGiftCard(Apple_Passes_Gift_Card_Model GiftCard)
        {
            try
            {

                    ResponseModel<string> res = await _Gift_Service.GenerateGiftCard(GiftCard);
                    if (res.Status_Code != "200")
                    {
                       return res;
                    }
                    var successResponse = new SuccessResponse
                    {
                        Message         = "Your Gift Card is saved successfully"
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
        [Route("GetMerchantsGiftCard")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetMerchantsGiftCard()
        {
            try
            {

                int tenantId                    = _httpContextAccessor.GetTenantId(); 
                IEnumerable<GiftCard> gifts     = await _Gift_Service.Find(x=>x.TenantId==tenantId);
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
        [Route("GetMerchantsGiftCardById")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetMerchantsGiftCardById(int Id)
        {
            try
            {

                int tenantId = _httpContextAccessor.GetTenantId();
                var gifts    = await _Gift_Service.FindOne(x => x.Id == Id && x.TenantId==tenantId);
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
        [Route("UpdateMerchantsGiftCard")]
        [CustomAuthorize("Update")]
        public async Task<dynamic> UpdateMerchantsGiftCard(Apple_Passes_Gift_Card_Model model)
        {
            try
            {

                var res= await _Gift_Service.UpdateGiftCard(model);
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
                    Message = "Your Gift Card is updated successfully"
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
        [Route("DeleteMerchantsGiftCard")]
        [CustomAuthorize("Delete")]
        public async Task<dynamic> DeleteMerchantsGiftCard(int Id)
        {
            try
            {

                int tenantId      = _httpContextAccessor.GetTenantId();
                var res           = await _Gift_Service.DeleteGiftCard(Id,tenantId);
                if (res.Status_Code == "200")
                {
                    await _Gift_Service.CompleteAync();
                    var successResponse = new SuccessResponse
                    {
                        Message = "Gift Card Deleted Successfully"
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
        [Route("DownloadGiftCard")]
        public async Task<dynamic> DownloadGiftCard(Apple_Passes_Gift_Card_Model GiftCard)
        {
            try
            {

                ResponseModel<string> res = await _Gift_Service.GenerateGiftCard(GiftCard);
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
