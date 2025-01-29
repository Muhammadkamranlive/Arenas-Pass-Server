using Server.Models;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.PassAssignment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassAssignmentController : ControllerBase
    {
        #region Constructor
        private readonly IAssign_Pass_Customer_Service _assign_Pass_Customer_Service;
        public PassAssignmentController
        (
           IAssign_Pass_Customer_Service assign_Pass_Customer
        )
        {
            _assign_Pass_Customer_Service = assign_Pass_Customer;
        }

        #endregion


        #region Pass Assignment 
        [HttpPost]
        [Route("AssignPassToCustomer")]
        public async Task<dynamic> AssignPassToCustomer(Assign_Pass_Model model)
        {
            try
            {
                ResponseModel<string> response= await _assign_Pass_Customer_Service.AssignGiftPassToCustomer(model);
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
        #endregion
    }
}
