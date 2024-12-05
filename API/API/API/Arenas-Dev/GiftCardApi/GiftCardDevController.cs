using Server.Core;
using Server.Models;
using Server.Domain;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.Arenas_Dev.GiftCardApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardDevController : ControllerBase
    {
        private readonly ITenant_Api_Hits_Service     tahService;
        private readonly ITenant_License_Keys_Service tlkService;
        private readonly ERPDb                        dbt;
        private readonly IGift_Card_Service           giftCardService;
        public GiftCardDevController
        (
            ITenant_Api_Hits_Service tah, 
            ITenant_License_Keys_Service tlk,
            IGift_Card_Service gift,
            ERPDb db
         )
        {
            tahService      = tah;
            tlkService      = tlk;
            giftCardService = gift;
            dbt             = db;
        }


        /// <summary>
        /// Get Gift Cards
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGiftCards")]
        public async Task<IActionResult> GetGiftCards()
        {
            try
            {
                // Extract public and private keys from headers
                if (!Request.Headers.TryGetValue("Public-Key", out var publicKeyValues) ||
                    !Request.Headers.TryGetValue("Private-Key", out var privateKeyValues))
                {
                    return Unauthorized(new
                    {
                        Error   = "Unauthorized",
                        Message = "Missing required headers 'Public-Key' or 'Private-Key'. Please include these in the request headers."
                    });
                }
                var publicKey  = publicKeyValues.FirstOrDefault();
                var privateKey = privateKeyValues.FirstOrDefault();

                // Validate keys against license service
                var license = await tlkService.FindOne(x=>x.publicKey==publicKey && x.privateKey==privateKey);
                if (license == null)
                {
                    return Unauthorized(new
                    {
                        Error   = "Invalid License",
                        Message = "The provided Keys doe not match any active license. Please check your license key."
                    });
                }

                // If valid, log the API hit and return gift cards
                var gifts = await giftCardService.Find(x=>x.TenantId==license.TenantId);
                if (gifts.Count != 0)
                {
                    TenantApiHitsHistory obj = new TenantApiHitsHistory
                    {
                        privateKey = license.privateKey,
                        publicKey  = license.publicKey,
                        TenantId   = license.TenantId,
                    };

                    dbt.TenantApiHitsHistories.Add(obj);
                    await dbt.SaveChangesAsync();

                    
                }
                return Ok(gifts);

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



        /// <summary>
        /// Get Gift Cards
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBySerialNo")]
        public async Task<IActionResult> GetGiftCardBySerialNo(string SerialNo)
        {
            try
            {
                // Extract public and private keys from headers
                if (!Request.Headers.TryGetValue("Public-Key", out var publicKeyValues) ||
                    !Request.Headers.TryGetValue("Private-Key", out var privateKeyValues))
                {
                    return Unauthorized(new
                    {
                        Error   = "Unauthorized",
                        Message = "Missing required headers 'Public-Key' or 'Private-Key'. Please include these in the request headers."
                    });
                }
                var publicKey  = publicKeyValues.FirstOrDefault();
                var privateKey = privateKeyValues.FirstOrDefault();

                // Validate keys against license service
                var license = await tlkService.FindOne(x=>x.publicKey==publicKey && x.privateKey==privateKey);
                if (license == null)
                {
                    return Unauthorized(new
                    {
                        Error   = "Invalid License",
                        Message = "The provided Keys doe not match any active license. Please check your license key."
                    });
                }

                // If valid, log the API hit and return gift cards
                var gifts  = await giftCardService.FindOne(x=>x.TenantId==license.TenantId && x.Serial_Number==SerialNo);
                if (gifts!=null)
                {
                    TenantApiHitsHistory obj = new TenantApiHitsHistory
                    {
                        privateKey = license.privateKey,
                        publicKey  = license.publicKey,
                        TenantId   = license.TenantId,
                    };

                    dbt.TenantApiHitsHistories.Add(obj);
                    await dbt.SaveChangesAsync();

                    
                }
                return Ok(gifts);

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
