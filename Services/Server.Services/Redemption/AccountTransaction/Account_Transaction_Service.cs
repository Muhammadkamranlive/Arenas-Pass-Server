using System;
using Server.UOW;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Account_Transaction_Service : Base_Service<Account_Transaction>, IAccount_Transaction_Service
    {
        private readonly IGift_Card_Service      gift_Card_Service;
        private readonly ITransaction_No_Service transaction_No_Service;
        private readonly IGet_Tenant_Id_Service  tenant_Id_Service;
        private readonly IAuthManager            auth_Manager_Service;
        private readonly IValidate_Txn_Service   vtService;
        public Account_Transaction_Service
        (
            IUnitOfWork               unitOfWork, 
            IAccount_Transaction_Repo genericRepository,
            IGift_Card_Service        gift_Card,
            ITransaction_No_Service   transaction_No,
            IGet_Tenant_Id_Service    get_Tenant_Id,
            IAuthManager              authManager,
            IValidate_Txn_Service     vt
        ) : base(unitOfWork, genericRepository)
        {
            gift_Card_Service      = gift_Card;
            transaction_No_Service = transaction_No;
            tenant_Id_Service      = get_Tenant_Id;
            auth_Manager_Service   = authManager;
            vtService              = vt;
        }

        public async Task<ResponseModel<string>> RedeemGiftCard(Redeem_Gift_Card_Model redeem_Gift)
        {
            try
            {
                ResponseModel<string>       RedeemResponse= new ResponseModel<string>() { Status_Code="200",Description="OK"};
                var TenantId                = tenant_Id_Service.GetTenantId();
                var userId                  = tenant_Id_Service.GetUserId();
                var UserName                = tenant_Id_Service.GetUserName();
                var CompanyName             = tenant_Id_Service.GetCompanyName();
                var GiftCard                = await gift_Card_Service.FindOne(x=>x.Serial_Number==redeem_Gift.Card_Id.ToString());
                //Null Checks 
                RedeemResponse              = CatchExceptionNull(GiftCard);
                if (RedeemResponse.Status_Code!="200") { return RedeemResponse; }

                //Check status for txnModel Card
                RedeemResponse             = gift_Card_Service.ValidateGiftCardForRedemption(GiftCard);
                if(RedeemResponse.Status_Code!="200")
                { return RedeemResponse; }

                IList<Account_Transaction> PrevTxns = await Find(x => x.Card_Id == redeem_Gift.Card_Id && x.DrCrFlag=="D");
                decimal txnAmount                   = PrevTxns.Sum(x => x.Amount);
                Account_Transaction txnModel        = new()
                {
                    Tenant_Id               = TenantId.ToString(),
                    Amount                  = redeem_Gift.Amount,
                    Card_Id                 = redeem_Gift.Card_Id,
                    Card_Type               = "GiftCard",
                    Customer_First_Name     = redeem_Gift.Customer_First_Name,
                    Customer_Last_Name      = redeem_Gift.Customer_Last_Name,
                    Email                   = redeem_Gift.Email,
                    DrCrFlag                = "D",
                    Processor_Id            = userId,
                    Processor_Name          = UserName
                    
                };

                //Validate  Transaction
                RedeemResponse              = await vtService.ValidateTxn(txnModel,txnAmount);
                if (RedeemResponse.Status_Code != "200")
                {
                    return RedeemResponse;
                }

                //Update txnModel card status R
                txnModel.Txn_Type        = (string)RedeemResponse.Response;

                //Pass Redemption
                var NewTxNo              = await transaction_No_Service.GetTxnNo();
                GiftCard.Pass_Status     = txnModel.Txn_Type;
                GiftCard.Recipient_Name  = txnModel.Customer_First_Name + "" + txnModel.Customer_Last_Name;
                GiftCard.Email           = txnModel.Email;
                GiftCard.Sender_Name     = CompanyName;
                GiftCard.Serial_Number   = NewTxNo.ToString() ;
                //update
                gift_Card_Service.Update(GiftCard,x=>x.Pass_Status,x=>x.Email,x=>x.Sender_Name,x=>x.Recipient_Name);

                //add Transaction
                var item = await AddAsync(txnModel);
                if (item != null)
                {
                    await Commit();
                    RedeemResponse.Status_Code = "200";
                    RedeemResponse.Description = "OK Transaction is  successfull";
                }
                else
                {
                    await Rollback();
                    RedeemResponse.Status_Code = "500";
                    RedeemResponse.Description = "Transaction was not successfull";
                }
                return RedeemResponse;
            }
            catch (Exception ex)
            {
                await Rollback();
                return CatchException(ex);
            }
        }

        public async Task<ResponseModel<string>> RedeemGiftCardByUserId(Redeem_Gift_CardByUserId_Model model)
        {
            try
            {
                ResponseModel<string>       RedeemResponse= new ResponseModel<string>() { Status_Code="200",Description="OK"};
                var TenantId                = tenant_Id_Service.GetTenantId();
                var userId                  = tenant_Id_Service.GetUserId();
                var UserName                = tenant_Id_Service.GetUserName();
                var CompanyName             = tenant_Id_Service.GetCompanyName();
                var GiftCard                = await gift_Card_Service.FindOne(x=>x.Serial_Number== model.Card_Id.ToString());
                //Null Checks 
                RedeemResponse              = CatchExceptionNull(GiftCard);
                if (RedeemResponse.Status_Code!="200") { return RedeemResponse; }


                //getting user
                ApplicationUser UserDetail   = await auth_Manager_Service.FindById(model.CustomerId);
                //Null Checks 
                RedeemResponse = CatchExceptionNull(UserDetail);
                if (RedeemResponse.Status_Code != "200") { return RedeemResponse; }

                //Check status for txnModel Card
                RedeemResponse             = gift_Card_Service.ValidateGiftCardForRedemption(GiftCard);
                if(RedeemResponse.Status_Code!="200")
                { return RedeemResponse; }

                IList<Account_Transaction> PrevTxns = await Find(x => x.Card_Id == model.Card_Id && x.DrCrFlag=="D");
                decimal txnAmount                   = PrevTxns.Sum(x => x.Amount);
                Account_Transaction txnModel        = new()
                {
                    Tenant_Id               = TenantId.ToString(),
                    Amount                  = model.Amount,
                    Card_Id                 = model.Card_Id,
                    Card_Type               = "GiftCard",
                    
                    Customer_First_Name     = UserDetail.FirstName,
                    Customer_Last_Name      = UserDetail.LastName,
                    Email                   = UserDetail.Email,

                    DrCrFlag                = "D",
                    Processor_Id            = userId,
                    Processor_Name          = UserName,
                };

                //Validate  Transaction
                RedeemResponse              = await vtService.ValidateTxn(txnModel,txnAmount);
                if (RedeemResponse.Status_Code != "200")
                {
                    return RedeemResponse;
                }

                //Update txnModel card status R
                txnModel.Txn_Type        = (string)RedeemResponse.Response;

                //Pass Redemption
                var NewTxNo              = await transaction_No_Service.GetTxnNo();
                GiftCard.Pass_Status     = txnModel.Txn_Type;
                GiftCard.Recipient_Name  = txnModel.Customer_First_Name + "" + txnModel.Customer_Last_Name;
                GiftCard.Email           = txnModel.Email;
                GiftCard.Sender_Name     = CompanyName;
                GiftCard.Serial_Number   = NewTxNo.ToString() ;
                //update
                gift_Card_Service.Update(GiftCard,x=>x.Pass_Status,x=>x.Email,x=>x.Sender_Name,x=>x.Recipient_Name);

                //add Transaction
                var item = await AddAsync(txnModel);
                if (item != null)
                {
                    await Commit();
                    RedeemResponse.Status_Code = "200";
                    RedeemResponse.Description = "OK Transaction is  successfull";
                }
                else
                {
                    await Rollback();
                    RedeemResponse.Status_Code = "500";
                    RedeemResponse.Description = "Transaction was not successfull";
                }
                return RedeemResponse;
            }
            catch (Exception ex)
            {
                await Rollback();
                return CatchException(ex);
            }
        }
    }
}
