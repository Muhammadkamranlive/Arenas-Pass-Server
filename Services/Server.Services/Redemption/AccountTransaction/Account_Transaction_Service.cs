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
                var TenantId             = tenant_Id_Service.GetTenantId();
                var userId             = tenant_Id_Service.GetUserId();
                var UserName                = tenant_Id_Service.GetUserName();
                var CompanyName             = tenant_Id_Service.GetCompanyName();
                var GiftCard                = await gift_Card_Service.FindOne(x=>x.Id==redeem_Gift.Card_Id);
                //Null Checks 
                RedeemResponse                 = CatchExceptionNull(GiftCard);
                if (RedeemResponse.Status_Code!="200") { return RedeemResponse; }
               
                //Check status for txnModel Card
                RedeemResponse                 = gift_Card_Service.ValidateGiftCardForRedemption(GiftCard, redeem_Gift);
                if(RedeemResponse.Status_Code!="200")
                { return RedeemResponse; }

                IList<Account_Transaction> transactions = new List<Account_Transaction>();
                Account_Transaction txnModel        = new()
                {
                    Tenant_Id                       = TenantId.ToString(),
                    Amount                          = redeem_Gift.Amount,
                    Card_Id                         = Convert.ToInt32(GiftCard.Serial_Number),
                    Card_Type                       = "GiftCard",
                    Customer_First_Name             = redeem_Gift.Customer_First_Name,
                    Email                           = redeem_Gift.Email,
                    DrCrFlag                        = "D",
                    Processor_Id                    = userId,
                    Processor_Name                  = UserName,
                    RedemptionType                  = Account_Transaction_Type_GModel.DebitedBalanceFromGiftCard,
                    Txn_Type                        = Account_Transaction_Type_GModel.Debit,
                };
                transactions.Add(txnModel);
                //Credit In Merchant Vault
                Account_Transaction MerchantVault   = new()
                {
                    Tenant_Id                       = TenantId.ToString(),
                    Amount                          = redeem_Gift.Amount,
                    Card_Id                         = Convert.ToInt32(GiftCard.Serial_Number),
                    Card_Type                       = "GiftCard",
                    Customer_First_Name             = redeem_Gift.Customer_First_Name,
                    Email                           = redeem_Gift.Email,
                    DrCrFlag                        = "C",
                    Processor_Id                    = userId,
                    Processor_Name                  = UserName,
                    RedemptionType                  = Account_Transaction_Type_GModel.CreditInMerchantVault,
                    Txn_Type                        = Account_Transaction_Type_GModel.Credit,
                };
                transactions.Add(MerchantVault);
                
                //Validate  Transaction
                if (GiftCard.Balance != redeem_Gift.Amount)
                {
                    RedeemResponse.Status_Code = "404";
                    RedeemResponse.Description = "Transaction amount dose not match with gift card balance";
                    return RedeemResponse;
                }
                else
                {
                    GiftCard.Balance = GiftCard.Balance-redeem_Gift.Amount;
                }
                //Pass TransactionType
                GiftCard.Pass_Status     = Pass_Redemption_Status_GModel.FullRedeemed;
                GiftCard.Recipient_Name  = txnModel.Customer_First_Name ;
                GiftCard.Email           = txnModel.Email;
                GiftCard.Sender_Name     = CompanyName;
                gift_Card_Service.Update(GiftCard);
                
                var appuser= await auth_Manager_Service.FindById(userId);
                if (appuser==null)
                {
                    RedeemResponse.Status_Code = "404";
                    RedeemResponse.Description = "Merchant does not exist";
                }
                //Adding Vault
                Vault usersVault = new Vault()
                {
                    TenantId   = appuser.TenantId,
                    UserId     = appuser.Id,
                    Email      = appuser.Email,
                    Amount     = redeem_Gift.Amount,
                    VaultType  = VaultType.Merchant
                };
                var item1 = await user_vault_Service.AddReturn(usersVault);
                if (item1 == null)
                {
                    RedeemResponse.Status_Code = "500";
                    RedeemResponse.Description = "Transaction was not successfull";
                    return  RedeemResponse;
                }
                
                //add Transaction
                var item = await AddBulk(transactions);
                if (item.Count()>0)
                {
                    
                    RedeemResponse.Status_Code = "200";
                    RedeemResponse.Description = "OK Redemption is successful";
                }
                else
                {
                    RedeemResponse.Status_Code = "500";
                    RedeemResponse.Description = "Transaction was not successfull";
                }

                if (RedeemResponse.Status_Code=="200")
                {
                    await CompleteAync();
                }
                else
                {
                    await Rollback();
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
                var TenantId             = tenant_Id_Service.GetTenantId();
                var userId             = tenant_Id_Service.GetUserId();
                var UserName                = tenant_Id_Service.GetUserName();
                var CompanyName             = tenant_Id_Service.GetCompanyName();
                redeem_Gift.Redemption_Type = Pass_Redemption_Status_GModel.FullRedeemed;
                
                var GiftCard                = await gift_Card_Service.FindOne(x=>x.Id==redeem_Gift.Card_Id);
                //Null Checks 
                RedeemResponse              = CatchExceptionNull(GiftCard);
                if (RedeemResponse.Status_Code!="200") { return RedeemResponse; }
                //Tenant
                var Tenant = await tenants_Service.FindOne(x => x.CompanyId == TenantId);
                RedeemResponse = CatchExceptionNull(Tenant);
                if (RedeemResponse.Status_Code != "200") { return RedeemResponse; }
                
                //Check status for txnModel Card
                RedeemResponse              = gift_Card_Service.ValidateGiftCardForRedemption(GiftCard, redeem_Gift);
                if(RedeemResponse.Status_Code!="200")
                { return RedeemResponse; }
                
                IList<Account_Transaction> transactions = new List<Account_Transaction>();
                Account_Transaction txnModel        = new()
                {
                    Tenant_Id                       = TenantId.ToString(),
                    Amount                          = redeem_Gift.Amount,
                    Card_Id                         = Convert.ToInt32(GiftCard.Serial_Number),
                    Card_Type                       = "GiftCard",
                    Customer_First_Name             = redeem_Gift.Customer_First_Name,
                    Email                           = redeem_Gift.Email,
                    DrCrFlag                        = "D",
                    Processor_Id                    = userId,
                    Processor_Name                  = UserName,
                    RedemptionType                  = Account_Transaction_Type_GModel.DebitedBalanceFromGiftCard,
                    Txn_Type                        = Account_Transaction_Type_GModel.Debit,
                };
                transactions.Add(txnModel);
                //Credit In Merchant Vault
                Account_Transaction MerchantVault   = new()
                {
                    Tenant_Id                       = TenantId.ToString(),
                    Amount                          = redeem_Gift.Amount,
                    Card_Id                         = Convert.ToInt32(GiftCard.Serial_Number),
                    Card_Type                       = "GiftCard",
                    Customer_First_Name             = redeem_Gift.Customer_First_Name,
                    Email                           = redeem_Gift.Email,
                    DrCrFlag                        = "C",
                    Processor_Id                    = userId,
                    Processor_Name                  = UserName,
                    RedemptionType                  = Account_Transaction_Type_GModel.CreditAddedinCustomerVault,
                    Txn_Type                        = Account_Transaction_Type_GModel.Credit,
                };
                transactions.Add(MerchantVault);
                //Validate  Transaction
                if (GiftCard.Balance != redeem_Gift.Amount)
                {
                    RedeemResponse.Status_Code = "404";
                    RedeemResponse.Description = "Transaction amount dose not match with gift card balance";
                    return RedeemResponse;
                }
                else
                {
                    GiftCard.Balance = GiftCard.Balance-redeem_Gift.Amount;
                }
                
                //Pass TransactionType
                GiftCard.Pass_Status     = Pass_Redemption_Status_GModel.FullRedeemed;
                GiftCard.Recipient_Name  = txnModel.Customer_First_Name;
                GiftCard.Email           = txnModel.Email;
                GiftCard.Sender_Name     = Tenant.CompanyName;
                //update
                gift_Card_Service.Update(GiftCard);
                //Adding Vault
                Vault usersVault = new Vault()
                {
                    TenantId   = TenantId,
                    UserId     = userId,
                    Email      = GiftCard.Email,
                    Amount     = redeem_Gift.Amount,
                    VaultType  = VaultType.Customer
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
                var item = await AddBulk(transactions);
                if (item.Count()>0)
                {
                    RedeemResponse.Status_Code = "200";
                    RedeemResponse.Description = "OK Transaction is  successfull";
                }
                else
                {
                    RedeemResponse.Status_Code = "500";
                    RedeemResponse.Description = "Transaction was not successfull";
                }
                
                if (RedeemResponse.Status_Code=="200")
                {
                    await CompleteAync();
                }
                else
                {
                    await Rollback();
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
