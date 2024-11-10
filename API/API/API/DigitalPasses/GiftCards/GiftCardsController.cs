using Server.Models;
using Server.Services;
using Passbook.Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Passbook.Generator.Fields;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace API.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardsController : ControllerBase
    {
        #region Contructor
        private readonly IGift_Card_Service _Gift_Service;
        public GiftCardsController
        (
          IGift_Card_Service gcards_Service    
        )
        {
            _Gift_Service = gcards_Service;
        }
        #endregion

        [HttpPost]
        [Route("GiftCard")]
        public async Task<dynamic> GenerateGiftCard(Apple_Passes_Gift_Card_Model GiftCard)
        {
            try
            {

                    ResponseModel<string> res = await _Gift_Service.GenerateGiftCard(GiftCard);
                    if (res.Status_Code != "200")
                    {
                       return res;
                    }
                    byte[] generatedPass      = (byte[])res.Response;
                    return new FileContentResult(generatedPass, "application/vnd.apple.pkpass")
                    {
                        FileDownloadName   = "File123" + ".pkpass"
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
