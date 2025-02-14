using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public class Payment_Features_Service : Base_Service<PaymentFeatureList>, IPayment_Features_Service
    {

        private readonly IGet_Tenant_Id_Service _tenant_id_service;
        public Payment_Features_Service
        (
            IUnit_Of_Work_Repo       unitOfWork,
            IFeature_List_Repo       genericRepository,
            IGet_Tenant_Id_Service   get_Tenant
        ) : base(unitOfWork, genericRepository)
        {
            _tenant_id_service = get_Tenant;
        }

        public async Task<ResponseModel<string>> AddFeatures(Payment_Feature_Model model)
        {
           try
            {
                ResponseModel<string> response = new ResponseModel<string>() { Status_Code = "200", Description = "Payment plan is added successfully" };
                int tenantId                           = _tenant_id_service.GetTenantId();
                string userId                          = _tenant_id_service.GetUserId();
                PaymentFeatureList ObjFeature          = new PaymentFeatureList();
                ObjFeature.FeatureTitle                = model.FeatureTitle;
                ObjFeature.FeatureDescription          = model.FeatureDescription;
                ObjFeature.PaymentPlanId               = model.PaymentPlanId;
                ObjFeature.UserId                      = userId;
                
                ObjFeature = await AddReturn( ObjFeature );
                if (ObjFeature == null) 
                {
                   response.Status_Code = "404";
                   
                    
                    response.Description = "Error in Adding Feature";
                   
                    
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

        public async Task<ResponseModel<string>> UpdateFeatures(Payment_Feature_Model model)
        {
           try
            {
                ResponseModel<string> response         = new ResponseModel<string>() { Status_Code = "200", Description = "Payment plan is added successfully" };
                int tenantId                           = _tenant_id_service.GetTenantId();
                string userId                          = _tenant_id_service.GetUserId();
                PaymentFeatureList ObjFeature          = await FindOne(x=>x.Id==model.Id);
                if (ObjFeature == null) 
                {
                    response.Status_Code = "404";
                    response.Description = "Record not found";
                    return response;
                }

                ObjFeature.FeatureTitle                = model.FeatureTitle;
                ObjFeature.FeatureDescription          = model.FeatureDescription;
                ObjFeature.PaymentPlanId               = model.PaymentPlanId;
                ObjFeature.UserId                      = userId;
                ObjFeature.Updated_At                  = DateTime.UtcNow;
                Update(ObjFeature);
                if (ObjFeature == null) 
                {
                    response.Status_Code = "404";
                    response.Description = "Error in Updating of Feature";
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
