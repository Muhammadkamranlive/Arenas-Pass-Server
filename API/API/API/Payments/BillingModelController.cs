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


        #region Features
        /// <summary>
        /// add payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPaymentFeatures")]
        public async Task<dynamic> AddPaymentFeatures(Payment_Feature_Model model)
        {
            try
            {
                 ResponseModel<string> response=  await _Payment_Features_Service.AddFeatures(model);
                 if (response.Status_Code != "200")
                 {
                     return BadRequest(response);
                 }
                 else
                 {
                     return Ok(response);
                 }
            }
            catch (Exception e)
            {
               return StatusCode(500, e.Message);
            }
        }
        
        
        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdatePaymentFeatures")]
        public async Task<dynamic> UpdatePaymentFeatures(Payment_Feature_Model model)
        {
            try
            {
                ResponseModel<string> response=  await _Payment_Features_Service.UpdateFeatures(model);
                if (response.Status_Code != "200")
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("DeletePaymentFeatures")]
        public async Task<dynamic> DeletePaymentFeatures(int Id)
        {
            try
            {
                await _Payment_Features_Service.Delete(Id);
                int count  =  await _Payment_Features_Service.CompleteAync();
                ResponseModel<string> response = new ResponseModel<string>();
                if (count == 0)
                {
                    response.Status_Code = "400";
                    response.Description = "Payment features not found";
                }
                else
                {
                    response.Status_Code = "200";
                    response.Description = "Payment features successfully deleted";
                }
                return Ok(response);
                
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("GetPaymentFeaturesById")]
        public async Task<dynamic> GetPaymentFeaturesById(int Id)
        {
            try
            {
                var paymentfeatures= await _Payment_Features_Service.FindOne(x=>x.Id==Id);
                return Ok(paymentfeatures);
                
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("GetPaymentFeatures")]
        public async Task<dynamic> GetPaymentFeatures(int paymentPlanId)
        {
            try
            {
                var paymentfeatures= await _Payment_Features_Service.Find(x => x.PaymentPlanId == paymentPlanId);
                return Ok(paymentfeatures);
                
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Tenant Charges
        /// <summary>
        /// add payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPaymentFeatures")]
        public async Task<dynamic> AddTenantCharges(Payment_Feature_Model model)
        {
            try
            {
                ResponseModel<string> response = await _Tenant_Charges_Service.AddFeatures(model);
                if (response.Status_Code != "200")
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdatePaymentFeatures")]
        public async Task<dynamic> UpdatePaymentFeatures(Payment_Feature_Model model)
        {
            try
            {
                ResponseModel<string> response = await _Payment_Features_Service.UpdateFeatures(model);
                if (response.Status_Code != "200")
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("DeletePaymentFeatures")]
        public async Task<dynamic> DeletePaymentFeatures(int Id)
        {
            try
            {
                await _Payment_Features_Service.Delete(Id);
                int count = await _Payment_Features_Service.CompleteAync();
                ResponseModel<string> response = new ResponseModel<string>();
                if (count == 0)
                {
                    response.Status_Code = "400";
                    response.Description = "Payment features not found";
                }
                else
                {
                    response.Status_Code = "200";
                    response.Description = "Payment features successfully deleted";
                }
                return Ok(response);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("GetPaymentFeaturesById")]
        public async Task<dynamic> GetPaymentFeaturesById(int Id)
        {
            try
            {
                var paymentfeatures = await _Payment_Features_Service.FindOne(x => x.Id == Id);
                return Ok(paymentfeatures);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// udate payment features 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("GetPaymentFeatures")]
        public async Task<dynamic> GetPaymentFeatures(int paymentPlanId)
        {
            try
            {
                var paymentfeatures = await _Payment_Features_Service.Find(x => x.PaymentPlanId == paymentPlanId);
                return Ok(paymentfeatures);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion


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
        [Route("DeletePaymentPlan")]
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
