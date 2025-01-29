using Server.Models;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Redemption
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedemptionController : ControllerBase
    {
        private readonly IAccount_Transaction_Service  act_Service;
        private readonly IGet_Tenant_Id_Service        get_Tenant_Id_Service;
        public RedemptionController
        (
           IAccount_Transaction_Service transaction ,IGet_Tenant_Id_Service get_Tenant_Id
        )
        {
            act_Service = transaction;
            get_Tenant_Id_Service = get_Tenant_Id;
        }

        /// <summary>
        /// Redemption
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RedeemGiftCard")]
        [CustomAuthorize("Write")]
        public async Task<dynamic> RedeemGiftCard(Redeem_Gift_Card_Model model)
        {
            try
            {
                ResponseModel<string> response= await act_Service.RedeemGiftCard(model);
                if (response.Status_Code != "200")
                {
                    return BadRequest(response.Description);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Redemption
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RedeemOnlineGiftCard")]
        [CustomAuthorize("Write")]
        public async Task<dynamic> RedeemOnlineGiftCard(Redeem_Gift_Card_Model model)
        {
            try
            {
                ResponseModel<string> response = await act_Service.RedeemGiftCardOnline(model);
                if (response.Status_Code != "200")
                {
                    return BadRequest(response.Description);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// Redeem GiftCard By CustomerId
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Route("RedeemGiftCardByCustomerId")]
        //public async Task<dynamic> RedeemGiftCardByCustomerId(Redeem_Gift_CardByUserId_Model model)
        //{
        //    try
        //    {
        //        ResponseModel<string> response = await act_Service.RedeemGiftCardByUserId(model);
        //        if (response.Status_Code != "200")
        //        {
        //            return BadRequest(response);
        //        }
        //        else
        //        {
        //            return Ok(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}


        /// <summary>
        /// Redeem GiftCard ByCustomerEmail
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Route("RedeemGiftCardByCustomerEmail")]
        //public async Task<dynamic> RedeemGiftCardByCustomerEmail(Redeem_Gift_CardByEmail_Model model)
        //{
        //    try
        //    {
        //        ResponseModel<string> response = await act_Service.RedeemGiftCardByEmail(model);
        //        if (response.Status_Code != "200")
        //        {
        //            return BadRequest(response);
        //        }
        //        else
        //        {
        //            return Ok(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
