using Azure;
using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public class Arenas_Payment_Plans_Service : Base_Service<Payment_Plans>, IArenas_Payment_Plans_Service
    {
        private readonly IGet_Tenant_Id_Service _tenant_id_service;
        public Arenas_Payment_Plans_Service
        (
            IUnit_Of_Work_Repo unitOfWork,
            IGet_Tenant_Id_Service get_Tenant_Id,
            IRepo<Payment_Plans> genericRepository
        ) : base(unitOfWork, genericRepository)
        {
            _tenant_id_service = get_Tenant_Id;
        }


        /// <summary>
        /// Make Payment Plans
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> Add(PaymentPlansModel model)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>() { Status_Code = "200", Description = "Payment plan is added successfully" };
                int tenantId                           = _tenant_id_service.GetTenantId();
                string userId                          = _tenant_id_service.GetUserId();
                Payment_Plans payment_Plans            = new Payment_Plans();
                payment_Plans.Plan_Name                = model.Plan_Name;
                payment_Plans.Max_Cards                = model.Max_Cards;
                payment_Plans.Price                    = model.Price;
                payment_Plans.Billing_Cycle            = model.Billing_Cycle;
                payment_Plans.Currency_Code            = model.Currency_Code;
                payment_Plans.Supports_Custom_Branding = model.Supports_Custom_Branding;
                payment_Plans.Is_Active                = model.Is_Active;
                payment_Plans.Is_Deleted               = false;
                payment_Plans.Trial_Period_Days        = model.Trial_Period_Days;
                payment_Plans.UserId                   = userId;
                
                payment_Plans = await AddReturn( payment_Plans );
                if (payment_Plans == null) 
                {
                   response.Status_Code = "404";
                   
                    
                    response.Description = "Error in adding gift card";
                   
                    
                    return response;
                }

                //commit
                if (response.Status_Code != "200")
                {
                    await Rollback();
                }
                await CompleteAync();
                return response;

            }
            catch (Exception ex)
            {
                await Rollback();
                return CatchException(ex);
            }
        }


        /// <summary>
        /// Update Payement Plans
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> UpdatePaymentPlan(PaymentPlansModel model)
        {
            try
            {
                ResponseModel<string> response         = new ResponseModel<string>() { Status_Code = "200", Description = "Payment plan is added successfully" };
                int tenantId                           = _tenant_id_service.GetTenantId();
                string userId                          = _tenant_id_service.GetUserId();
                Payment_Plans payment_Plans            = await FindOne(x=>x.Id==model.Id);
                if (payment_Plans == null) 
                {
                    response.Status_Code = "404";
                    response.Description = "Record not found";
                    return response;
                }

                payment_Plans.Plan_Name                = model.Plan_Name;
                payment_Plans.Max_Cards                = model.Max_Cards;
                payment_Plans.Price                    = model.Price;
                payment_Plans.Billing_Cycle            = model.Billing_Cycle;
                payment_Plans.Currency_Code            = model.Currency_Code;
                payment_Plans.Supports_Custom_Branding = model.Supports_Custom_Branding;
                payment_Plans.Is_Active                = model.Is_Active;
                payment_Plans.Is_Deleted               = false;
                payment_Plans.Trial_Period_Days        = model.Trial_Period_Days;
                payment_Plans.UserId                   = userId;
                payment_Plans.Updated_At               = DateTime.UtcNow;
                
                Update(payment_Plans);
                if (payment_Plans == null) 
                {
                    response.Status_Code = "404";
                    response.Description = "Record not found";
                    return response;
                }

                //commit
                if (response.Status_Code != "200")
                {
                    await Rollback();
                }
                await CompleteAync();
                return response;

            }
            catch (Exception ex)
            {
                await Rollback();
                return CatchException(ex);
            }
        }
    }
}
