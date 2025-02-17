using System;
using Server.UOW;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Server.Services
{
    public class UserBilling_Address_Service : Base_Service<UserBillingDetail>, IUserBilling_Address_Service
    {
        public UserBilling_Address_Service(IUnit_Of_Work_Repo unitOfWork, IUserBilling_Address_Repo genericRepository) : base(unitOfWork, genericRepository)
        {
        }


        public async Task<ResponseModel<string>> AddOrUpdateBilingDetail(UserBillingDetail model)
        {
            try
            {
                   ResponseModel<string> BillingResponse = new ResponseModel<string>() { Description="OK",Status_Code="200"};
                    var Billing = await FindOne(x=>x.Email==model.Email);
                    if (Billing != null) 
                    {
                       Billing = model;
                       Update(model);
                       await CompleteAync();
                      BillingResponse.Status_Code = "200";
                      BillingResponse.Description = "OK Billing detail is updated successfully";
                    }
                    else
                    {
                        var response = await AddReturn(model);
                        if (response != null) 
                        {
                           await CompleteAync();
                           BillingResponse.Status_Code = "200";
                           BillingResponse.Description = "OK Billing detail is  added successfully";
                        }
                    }

                   return BillingResponse;
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }
        }


    }
}
