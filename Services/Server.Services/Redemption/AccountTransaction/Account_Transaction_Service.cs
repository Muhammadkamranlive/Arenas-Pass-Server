using System;
using Server.UOW;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using Server.Configurations;
using System.Security.Cryptography.X509Certificates;

namespace Server.Services
{
    public class Account_Transaction_Service : Base_Service<Account_Transaction>, IAccount_Transaction_Service
    {
        private readonly IGift_Card_Service      gift_Card_Service;
        private readonly ITransaction_No_Service transaction_No_Service;
        private readonly IGet_Tenant_Id_Service  tenant_Id_Service;
        private readonly IAuth_Manager_Service            auth_Manager_Service;
        private readonly IValidate_Txn_Service   vtService;
        private readonly IUser_Vault_Service      user_vault_Service;
        private readonly ITenants_Service        tenants_Service;
        public Account_Transaction_Service
        (
            IUnit_Of_Work_Repo               unitOfWork, 
            IAccount_Transaction_Repo genericRepository,
            IGift_Card_Service        gift_Card,
            ITransaction_No_Service   transaction_No,
            IGet_Tenant_Id_Service    get_Tenant_Id,
            IAuth_Manager_Service              authManager,
            IValidate_Txn_Service     vt,
            IUser_Vault_Service        user_vault,
            ITenants_Service           tenants
        ) : base(unitOfWork, genericRepository)
        {
            gift_Card_Service      = gift_Card;
            transaction_No_Service = transaction_No;
            tenant_Id_Service      = get_Tenant_Id;
            auth_Manager_Service   = authManager;
            vtService              = vt;
            user_vault_Service     = user_vault;
            tenants_Service        = tenants;
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
                var GiftCard                = await gift_Card_Service.FindOne(x=>x.Id==redeem_Gift.Card_Id);
                //Null Checks 
                RedeemResponse              = CatchExceptionNull(GiftCard);
                if (RedeemResponse.Status_Code!="200") { return RedeemResponse; }
               
                //Check status for txnModel Card
                RedeemResponse             = gift_Card_Service.ValidateGiftCardForRedemption(GiftCard, redeem_Gift);
                if(RedeemResponse.Status_Code!="200")
                { return RedeemResponse; }

                IList<Account_Transaction> PrevTxns = await Find(x => x.Card_Id.ToString() == GiftCard.Serial_Number && x.DrCrFlag=="D");
                decimal txnAmount                   = PrevTxns.Sum(x => x.Amount);
                Account_Transaction txnModel        = new()
                {
                    Tenant_Id                       = TenantId.ToString(),
                    Amount                          = redeem_Gift.Amount,
                    Card_Id                         = Convert.ToInt32(GiftCard.Serial_Number),
                    Card_Type                       = "GiftCard",
                    Customer_First_Name             = redeem_Gift.Customer_First_Name,
                    Customer_Last_Name              = redeem_Gift.Customer_Last_Name,
                    Email                           = redeem_Gift.Email,
                    DrCrFlag                        = "D",
                    Processor_Id                    = userId,
                    Processor_Name                  = UserName,
                    RedemptionType                  = RedemptionStatusModel.InStore 
                    
                };

                //Validate  Transaction
                RedeemResponse                  = await vtService.ValidateTxn(txnModel,txnAmount);
                if (RedeemResponse.Status_Code != "200")
                {
                    return RedeemResponse;
                }

                //Update txnModel card status R
                txnModel.Txn_Type        = (string)RedeemResponse.Response;

                //Pass TransactionType
                var NewTxNo              = await transaction_No_Service.GetTxnNo();
                GiftCard.Pass_Status     = txnModel.Txn_Type;
                GiftCard.Recipient_Name  = txnModel.Customer_First_Name ;
                GiftCard.Email           = txnModel.Email;
                GiftCard.Sender_Name     = CompanyName;
                //update balance 
                GiftCard.Balance         = Convert.ToDecimal(RedeemResponse.Description);
                //update
                gift_Card_Service.Update(GiftCard);

                //add Transaction
                var item = await AddReturn(txnModel);
                if (item != null)
                {
                    await CompleteAync();
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

        public async Task<ResponseModel<string>> RedeemGiftCardOnline(Redeem_Gift_Card_Model redeem_Gift)
        {
             try
            {
                ResponseModel<string>       RedeemResponse= new ResponseModel<string>() { Status_Code="200",Description="OK"};
                var TenantId                = tenant_Id_Service.GetTenantId();
                var userId                  = tenant_Id_Service.GetUserId();
                var UserName                = tenant_Id_Service.GetUserName();
                var CompanyName             = tenant_Id_Service.GetCompanyName();
                redeem_Gift.Redemption_Type = Pass_Redemption_Status_GModel.FullRedeemed;
                var AppUser                 = await auth_Manager_Service.FindById(userId);
                RedeemResponse              = CatchExceptionNull(AppUser);
                if (RedeemResponse.Status_Code != "200") { return RedeemResponse; }

                var GiftCard                = await gift_Card_Service.FindOne(x=>x.Id==redeem_Gift.Card_Id);
                //Null Checks 
                RedeemResponse              = CatchExceptionNull(GiftCard);
                if (RedeemResponse.Status_Code!="200") { return RedeemResponse; }
                //Tenant
                var Tenant = await tenants_Service.FindOne(x => x.CompanyId == TenantId);
                RedeemResponse = CatchExceptionNull(Tenant);
                if (RedeemResponse.Status_Code != "200") { return RedeemResponse; }

                //Check status for txnModel Card
                RedeemResponse             = gift_Card_Service.ValidateGiftCardForRedemption(GiftCard, redeem_Gift);
                if(RedeemResponse.Status_Code!="200")
                { return RedeemResponse; }

                IList<Account_Transaction> PrevTxns = await Find(x => x.Card_Id.ToString() == GiftCard.Serial_Number && x.DrCrFlag=="D");
                decimal txnAmount                   = PrevTxns.Sum(x => x.Amount);
                Account_Transaction txnModel        = new()
                {
                    Tenant_Id               = TenantId.ToString(),
                    Amount                  = redeem_Gift.Amount,
                    Card_Id                 = Convert.ToInt32(GiftCard.Serial_Number),
                    Card_Type               = "GiftCard",
                    Customer_First_Name     = redeem_Gift.Customer_First_Name,
                    Customer_Last_Name      = redeem_Gift.Customer_Last_Name,
                    Email                   = redeem_Gift.Email,
                    DrCrFlag                = "D",
                    Processor_Id            = userId,
                    Processor_Name          = UserName,
                    RedemptionType          = RedemptionStatusModel.Vault,
                };

                //Validate  Transaction
                RedeemResponse              = await vtService.ValidateTxn(txnModel,txnAmount);
                if (RedeemResponse.Status_Code != "200")
                {
                    return RedeemResponse;
                }

                //Update txnModel card status R
                txnModel.Txn_Type        = (string)RedeemResponse.Response;

                //Pass TransactionType
                var NewTxNo              = await transaction_No_Service.GetTxnNo();
                GiftCard.Pass_Status     = txnModel.Txn_Type;
                GiftCard.Recipient_Name  = txnModel.Customer_First_Name + "" + txnModel.Customer_Last_Name;
                GiftCard.Email           = txnModel.Email;
                GiftCard.Sender_Name     = CompanyName;
                //update balance 
                GiftCard.Balance         = Convert.ToDecimal(RedeemResponse.Description);
                //update
                gift_Card_Service.Update(GiftCard);

                //Adding Vault
                UsersVault usersVault = new UsersVault()
                {
                    TenantId   = TenantId,
                    UserId     = userId,
                    UserEmail  = GiftCard.Email,
                    Amount     = GiftCard.Balance,
                    VaultType = VaultType.Merchant

                };

                var item1 = await user_vault_Service.AddReturn(usersVault);
                if (item1 == null)
                {
                    await Rollback();
                    RedeemResponse.Status_Code = "500";
                    RedeemResponse.Description = "Transaction was not successfull";
                    return  RedeemResponse;
                }
                
               

                //add Transaction
                var item = await AddReturn(txnModel);
                if (item != null)
                {
                    await CompleteAync();
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
