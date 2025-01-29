using Stripe;
using Server.Models;
using Server.Domain;
using Server.Services;
using Passbook.Generator;
using Server.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Passbook.Generator.Fields;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Drawing.BarCodes;
using System.Security.Cryptography.X509Certificates;

namespace API.API.DigitalPasses
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardsController : ControllerBase
    {
        #region Contructor
        private readonly IGift_Card_Service           _Gift_Service;
        private readonly IGet_Tenant_Id_Service       _httpContextAccessor;
        private readonly IAccount_Transaction_Service _Transaction_Service;
        private readonly IAccount_Balance_Service     _Balance_Service;
        public GiftCardsController
        (
          IGift_Card_Service gcards_Service, IGet_Tenant_Id_Service httpContext,
          IAccount_Transaction_Service     account_Transaction,
          IAccount_Balance_Service         account_Balance
        )
        {
            _Gift_Service        = gcards_Service;      _httpContextAccessor = httpContext;
            _Transaction_Service = account_Transaction;
            _Balance_Service     = account_Balance;
        }
        #endregion

        [HttpPost]
        [Route("GenerateCard")]
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
                        Message         = "Your Gift Card Template is saved successfully"
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
                IEnumerable<GiftCard> gifts     = await _Gift_Service.Find(x=>x.TenantId==tenantId && x.Pass_Status==Pass_Redemption_Status_GModel.Template);
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
        [Route("GetGiftCardWithCustomerEmail")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetGiftCardWithCustomerEmail(string email)
        {
            try
            {
                IEnumerable<GiftCard> gifts = await _Gift_Service.Find(x => x.Email==email);
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
        [Route("GetGiftCardBySerialNo")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetGiftCardBySerialNo(string SerialNo)
        {
            try
            {

                GiftCard gifts        = await _Gift_Service.FindOne(x =>  x.Pass_Status != Pass_Redemption_Status_GModel.Template && x.Serial_Number==SerialNo);
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
        [Route("GiftCardTransactions")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GiftCardTransactions(string SerialNo)
        {
            try
            {

                int tenantId   = _httpContextAccessor.GetTenantId();
                var gifts      = await _Transaction_Service.Find(x => x.Tenant_Id == tenantId.ToString() && x.Card_Id == Convert.ToUInt32(SerialNo));
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
        [Route("GiftCardBalance")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GiftCardBalance(string SerialNo)
        {
            try
            {

                int tenantId = _httpContextAccessor.GetTenantId();
                var gifts    = await _Balance_Service.FindOne(x =>  x.ACCOUNT_NO == SerialNo);
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
        [Route("GetMerchantsRedeemableGiftCard")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetMerchantsRedeemableGiftCard()
        {
            try
            {

                int tenantId = _httpContextAccessor.GetTenantId();
                IEnumerable<GiftCard> gifts = await _Gift_Service.Find(x => x.TenantId == tenantId && x.Pass_Status != Pass_Redemption_Status_GModel.Template);
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

        [HttpGet]
        [Route("GetCardById")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetGiftCardById(int Id)
        {
            try
            {

                int tenantId  = _httpContextAccessor.GetTenantId();
                var  gifts    = await _Gift_Service.FindOne(x => x.Id == Id && x.TenantId == tenantId);
                Apple_Passes_Gift_Card_Model gift = new Apple_Passes_Gift_Card_Model()
                {
                    Id                   = gifts.Id,
                    Background_Color     = gifts.Background_Color,
                    Foreground_Color     = gifts.Foreground_Color,
                    Label_Color          = gifts.Label_Color,
                    Expiration_Date      = gifts.Expiration_Date,
                    Logo_Url             = gifts.Logo_Url,
                    Logo_Text            = gifts.Logo_Text,
                    Localized_Name       = gifts.Localized_Name,
                    Card_holder_Name     = gifts.Card_holder_Name,
                    Card_Holder_Title    = gifts.Card_Holder_Title,
                    Code_Type            = gifts.Code_Type,
                    Recipient_Name       = gifts.Recipient_Name,
                    Sender_Name          = gifts.Sender_Name,
                    Email                = gifts.Email,
                    Phone                = gifts.Phone,
                    Address              = gifts.Address,
                    Privacy_Policy       = gifts.Privacy_Policy,
                    Terms_And_Conditions = gifts.Terms_And_Conditions,
                    Message              = gifts.Message,
                    Description          = gifts.Description,
                    Balance              = gifts.Balance,
                    Currency_Code        = gifts.Currency_Code,
                    Currency_Sign        = gifts.Currency_Sign,

                };
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
                    Message = "Your Gift Card Template is updated successfully"
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
                        Message = "Gift Card Template Deleted Successfully"
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
                    Response    = "Error Occurred"
                };
                return giftResponse;
            }
        }

    }
}
