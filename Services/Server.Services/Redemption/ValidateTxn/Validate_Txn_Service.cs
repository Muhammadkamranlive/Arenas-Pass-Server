﻿using System.Text;
using Server.Domain;
using Server.Models;
using Server.Configurations;

namespace Server.Services
{
    public class Validate_Txn_Service:IValidate_Txn_Service
    {

        private readonly IAccount_Balance_Service     actBalance_Service;
        public Validate_Txn_Service
        (
           IAccount_Balance_Service     actBal
        )
        {

            actBalance_Service = actBal;

        }

        /// <summary>
        /// Validate Txn
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<string>> ValidateTxn(Account_Transaction account_Transaction, decimal PrevtxnAmount)
        {
            try
            {
                ResponseModel<string> responseModel = new ResponseModel<string>() { Status_Code = "200", Description = "OK", Response = "OK" };
                //CheckAccount Balance 
                Account_Balance account_Balance     = await actBalance_Service.FindOne(x=>x.ACCOUNT_NO==account_Transaction.Card_Id.ToString() && x.Tenant_Id.ToString() == account_Transaction.Tenant_Id);
                responseModel                       = CatchExceptionNull(account_Balance);
                
                if (responseModel.Status_Code != "200")
                {
                    responseModel.Description   = "Error card is not redeemable assign it to customer";
                    responseModel.Status_Code   = "400";
                    return responseModel;
                }

                //validate Transaction Discount_Percentage
                decimal  BalanceAmount = account_Balance.Amount;
                BalanceAmount          = BalanceAmount - PrevtxnAmount;
                if (BalanceAmount < account_Transaction.Amount)
                {
                    responseModel.Description = "Transaction amount should not exceed from available amount";
                    responseModel.Status_Code = "400";
                    return responseModel;
                }

                else if (account_Balance.Account_Status == "closed")
                {
                    responseModel.Description = account_Transaction.Card_Type + "is already redeemed";
                    responseModel.Status_Code = "400";
                    return responseModel;
                }

                else if (BalanceAmount == account_Transaction.Amount)
                {
                    responseModel.Description = (BalanceAmount - account_Transaction.Amount).ToString();
                    responseModel.Response    = Pass_Redemption_Status_GModel.FullRedeemed;
                    responseModel.Status_Code = "200";
                }
                else if(BalanceAmount > account_Transaction.Amount)
                {
                    responseModel.Description = (BalanceAmount -account_Transaction.Amount).ToString();
                    responseModel.Response    = Pass_Redemption_Status_GModel.PartialyRedeemed;
                    responseModel.Status_Code = "200";
                    return responseModel;
                }


                
                return responseModel;
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }
        }



        private ResponseModel<string> CatchException(Exception ex)
        {
            ResponseModel<string> giftResponse = new()
            {
                Status_Code = "500",
                Description = ex.Message,
                Response    = "Error Occurred"
            };
            return giftResponse;
        }

        /// <summary>
        /// Catch NUll
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResponseModel<string> CatchExceptionNull(dynamic entity)
        {
            ResponseModel<string> giftResponse = new ResponseModel<string>()
            {
                Status_Code = "200",
                Description = "OK",
                Response = "OK"
            };
            if (entity == null)
            {
                giftResponse = new ResponseModel<string>()
                {
                    Status_Code = "400",
                    Description = "Error Could not find record",
                    Response = "Error Occurred"
                };
            }

            return giftResponse;
        }
    }
}
