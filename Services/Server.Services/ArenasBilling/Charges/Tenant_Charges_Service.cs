using System;
using Server.UOW;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;
using Server.Models;

namespace Server.Services
{
    public class Tenant_Charges_Service : Base_Service<TenantCharges>, ITenant_Charges_Service
    {
        private readonly ITenant_Charges_Repo _tenant_Charges_Repo;
        private readonly IGet_Tenant_Id_Service _get_Tenant_Id_Service;
        public Tenant_Charges_Service
        (
            IUnit_Of_Work_Repo unitOfWork, 
            ITenant_Charges_Repo repo,
            IGet_Tenant_Id_Service get_Tenant_Id_Service
        ) : base(unitOfWork, repo)
        {
            _tenant_Charges_Repo = repo;
            _get_Tenant_Id_Service = get_Tenant_Id_Service;
        }


        public async Task<ResponseModel<string>> AddCharges(ChargesModel model)
        {
            try
            {
                ResponseModel<string> responseModel = new ResponseModel<string>(){Status_Code = "200",Description = "OK"};
                TenantCharges obj= new TenantCharges();
                obj.ChargeAmount = model.ChargeAmount;
                obj.Status = model.Status;
                obj.ChargeType = model.ChargeType;
                obj.ChargeDescription = model.ChargeDescription;
                obj.ChargeName = model.ChargeName;
                obj.ChargeType = model.ChargeType;
                obj.UserId          = _get_Tenant_Id_Service.GetUserId();
                var response =await _tenant_Charges_Repo.AddReturn(obj);
                if (response!=null)
                {
                    responseModel.Status_Code = "200";
                   await CompleteAync();
                }
                return responseModel;
            }
            catch (Exception e)
            {
                return CatchException(e);
            }
        }

        public async Task<ResponseModel<string>> UpdateCharges(ChargesModel model)
        {
            try
            {
                ResponseModel<string> responseModel = new ResponseModel<string>(){Status_Code = "200",Description = "OK"};
                TenantCharges obj= await FindOne(x=>x.Id==model.Id);
                
                if (obj == null)
                {
                    responseModel.Status_Code = "404";
                    responseModel.Description = "Charges not found against id ";
                    return responseModel;
                }
                
           
                obj.ChargeAmount         = model.ChargeAmount;
                obj.Status               = model.Status;
                obj.ChargeType           = model.ChargeType;
                obj.ChargeDescription    = model.ChargeDescription;
                obj.ChargeName           = model.ChargeName;
    
                obj.ChargeType           = model.ChargeType;
           
                obj.UserId               = _get_Tenant_Id_Service.GetUserId();
                obj.UpdatedAt            = DateTime.Now;
                obj.Status               = model.Status;
                _tenant_Charges_Repo.Update(obj);
                await CompleteAync();
                return responseModel;
            }
            catch (Exception e)
            {
                return CatchException(e);
            }
        }
    }
}
