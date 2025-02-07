using Server.Models;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingModelController : ControllerBase
    {

        private readonly ITenant_Charges_Service        _Tenant_Charges_Service;
        private readonly IArenas_Payment_Plans_Service  _Payment_Plans_Service;
        private readonly IPayment_Features_Service      _Payment_Features_Service;
        public BillingModelController
        (
            ITenant_Charges_Service       tenant_Charges,
            IArenas_Payment_Plans_Service arenas_Payment_Plans,
            IPayment_Features_Service     payment_Features
        )
        {
            _Tenant_Charges_Service   = tenant_Charges;
            _Payment_Plans_Service    = arenas_Payment_Plans;
            _Payment_Features_Service = payment_Features;
        }



        #region Payment Plans

        /// <summary>
        /// Add Payement Plans
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("AddPayementPlan")]
        public async Task<dynamic> AddPayementPlan(PaymentPlansModel model)
        {
            try
            {
                ResponseModel<string> response = await _Payment_Plans_Service.Add(model);
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

        /// <summary>
        /// Update Payement Plans
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePaymenttPlan")]
        public async Task<dynamic> UpdatePaymenttPlan(PaymentPlansModel model)
        {
            try
            {
                ResponseModel<string> response = await _Payment_Plans_Service.UpdatePaymentPlan(model);
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


        /// <summary>
        /// Get All Payement Plans
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllPayementPlans")]
        public async Task<dynamic> GetAllPayementPlans()
        {
            try
            {
                var list = await _Payment_Plans_Service.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Get One Payement Plans
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayementPlanById")]
        public async Task<dynamic> GetAllPayementPlans(int Id)
        {
            try
            {
                var list = await _Payment_Plans_Service.FindOne(x=>x.Id==Id);
                return Ok(list);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete Payement Plans
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayementPlanById")]
        public async Task<dynamic> DeletePaymentPlan(int Id)
        {
            try
            {
                ResponseModel<string> response = new() { Status_Code = "200", Description = "Deletion success",Response=true };
                response.Response            = await _Payment_Plans_Service.Delete(Id);
                await _Payment_Plans_Service.CompleteAync(); 
                return Ok(response.Description);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
