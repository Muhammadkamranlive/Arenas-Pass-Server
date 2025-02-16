using Server.Models;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.PassTransmission
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassTransmissionController : ControllerBase
    {
        private readonly ISend_Pass_Customer_Service sendPass_Service;
        public PassTransmissionController
        (
          ISend_Pass_Customer_Service send_Pass_Customer
        )
        {
            sendPass_Service= send_Pass_Customer;
        }


        #region Pass Assignment 
        [HttpPost]
        [Route("SendPassToCustomer")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> SendPassToCustomer(IList<string> model)
        {
            try
            {
                ResponseModel<string> response = await sendPass_Service.SendtoUser(model);
                if (response.Status_Code != "200")
                {
                    return BadRequest(response);
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
        
        [HttpPost]
        [Route("SendPassToCustomerwithcustomMessage")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> SendPassToCustomerwithcustomMessage(IList<string> model,string message,string subject)
        {
            try
            {
                ResponseModel<string> response = await sendPass_Service.SendtoUser(model, message, subject);
                if (response.Status_Code != "200")
                {
                    return BadRequest(response);
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
        
        

        [HttpGet]
        [Route("GetPassToSend")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetPassToSend()
        {
            try
            {

                return Ok(await sendPass_Service.GetWalletPasses());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
