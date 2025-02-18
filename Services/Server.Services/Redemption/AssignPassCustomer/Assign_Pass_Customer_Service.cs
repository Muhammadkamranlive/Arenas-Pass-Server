using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using Server.Configurations;
using Server.Services.DigitalPasses;

namespace Server.Services
{
    public class Assign_Pass_Customer_Service : Base_Service<Account_Transaction>, IAssign_Pass_Customer_Service
    {
        #region Constructor
        private readonly IAccount_Transaction_Service _transactionService;
        private readonly IGet_Tenant_Id_Service       _Tenant_Id_Service;
        private readonly ITransaction_No_Service      _transaction_No_Service;
        private readonly IGift_Card_Service           _gift_Service;
        private readonly IAuth_Manager_Service        _authManager_Service;
        private readonly IAccount_Balance_Service     _account_Balance_Service;
        private readonly IWallet_Pass_Service         _wallet_Pass_Service;
        private readonly IMembership_Card_Service     _membership_Card_Service;
        private readonly ILoyalty_Card_Service        _loyalty_Card_Service;
        private readonly IPunch_Card_Service          _punch_Card_Service;
        private readonly IUser_Vault_Service          _vault_Service;
        private readonly ITenant_Charges_Service      _tenant_Charges_Service;
        private readonly ITenants_Service          _tenants_Service;
        public Assign_Pass_Customer_Service
        (   IUnit_Of_Work_Repo                  unitOfWork,
            IAccount_Transaction_Repo    genericRepository,
            IAccount_Transaction_Service account_Transaction,
            IGet_Tenant_Id_Service       get_Tenant_Id,
            ITransaction_No_Service      transaction_No,
            IGift_Card_Service           gift_Card,
            IAuth_Manager_Service        authManager,
            IWallet_Pass_Service         wallet_Pass,
            IAccount_Balance_Service     account_Balance,
            IMembership_Card_Service     membership_Card,
            ILoyalty_Card_Service        loyalty_Card,
            IPunch_Card_Service          punch_Card,
            IUser_Vault_Service              vault,
            ITenant_Charges_Service     tenant_Charges,
            ITenants_Service           tenants

        ) : base(unitOfWork, genericRepository)
        {
            _transactionService          = account_Transaction;
            _Tenant_Id_Service           = get_Tenant_Id;
            _transaction_No_Service      = transaction_No;
            _gift_Service                = gift_Card;
            _authManager_Service         = authManager;
            _account_Balance_Service     = account_Balance;
            _wallet_Pass_Service         = wallet_Pass;
            _membership_Card_Service     = membership_Card;
            _loyalty_Card_Service        = loyalty_Card;
            _punch_Card_Service          = punch_Card;
            _vault_Service                = vault;
            _tenant_Charges_Service      = tenant_Charges;
            _tenants_Service            = tenants;
        }
        #endregion



        /// <summary>
        /// Assign Pass
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> AssignGiftPassToCustomer(Assign_Pass_Model model)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>() { Status_Code = "200", Description="OK",Response="OK" };
                //Find Customer
                if (model.Customer_Type == "Registered")
                {
                    ApplicationUser Customer = await _authManager_Service.FindbyEmail(model.Customer_email);
                    response = CatchExceptionNull(Customer);
                    if (response.Status_Code != "200")
                    {
                        response.Description = response.Description + "Oops looks like customer is not registered";
                        return response;
                    }
                    model.Customer_email       = Customer.Email;
                    model.Customer_Name        = Customer.FirstName + " "+ Customer.LastName;
                    model.Phone                = Customer.PhoneNumber;
                    model.Customer_Type        = "Registered";
                }
                
                //find tenant
                var tenantName                 = _Tenant_Id_Service.GetCompanyName();
                var TenantId                = _Tenant_Id_Service.GetTenantId();
                var userId                = _Tenant_Id_Service.GetUserId();
                var userName                   = _Tenant_Id_Service.GetUserName();

                //find Wallet and Its Type
                WalletPass walletPass = await _wallet_Pass_Service.FindOne(x => x.Id == model.Card_Id);
                response              = CatchExceptionNull(walletPass);
                if (response.Status_Code != "200")
                {
                    response.Description= response.Description + "Gift Card";
                    return response;
                }
                
                //Wallet Found now make Txn No for it 
                int serialNo = await _transaction_No_Service.GetTxnNo();
                //Init Account Txn and Account Balance Domains
                
                //Type of Pass
                if (walletPass.Type== Pass_Type_GModel.GiftCard)
                {
                    //Fing Gift card
                    GiftCard   giftCard       = await _gift_Service.FindOne(x => x.Id == model.Card_Id);
                    giftCard.Recipient_Name   = model.Customer_Name;
                    giftCard.Email            = model.Customer_email;
                    giftCard.Phone            = model.Phone;
                    giftCard.Sender_Name      = model.Store_Name ?? tenantName;
                    giftCard.Address          = model.Store_Address;
                    giftCard.Pass_Status      = Pass_Redemption_Status_GModel.PendingForSend;
                    giftCard.Serial_Number    = serialNo.ToString();
                    giftCard.Id               = 0;
                    
                    //Check Merchant vault amount
                    IList<Vault> Merchantvault = await _vault_Service.Find(x=>x.TenantId == TenantId);
                    decimal vaultAmount        = Merchantvault.Sum(x=>x.Amount);
                    if (vaultAmount==0)
                    {
                        response.Status_Code = "404";
                        response.Description = "Merchant vault does not have sufficient vault amount please add balance in vault first";
                        return response;
                    }
                    else if (vaultAmount < giftCard.Balance)
                    {
                        response.Status_Code = "404";
                        response.Description = "Merchant vault amount is less than gift balance";
                        return response;
                    }
                    
                    //Get Redemption Charge Price
                    var charges = await _tenant_Charges_Service.FindOne(x => x.ChargeType ==ChargesTypeEnum.MerchantCharges.GiftCardRedemption);
                    if (charges == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Gift card charges does not exist";
                        return response;
                    }
                    
                    //giftcard plus redemption amount totall get remaining vault  
                    decimal totalChargesAndRedemptionAmount = (giftCard.Balance+charges.ChargeAmount.Value);
                    //Validate Gift Card amount with vault amount and redemption amount
                    if (vaultAmount <totalChargesAndRedemptionAmount)
                    {
                        response.Status_Code = "404";
                        response.Description = "vault amount is not enough to be deductible and cannot be redeemed it should be greater than gift card balance plus charges amount";
                        return response;
                    }
                    
                    //deduct amount from vault plus deduct redemption amount from vault 
                    vaultAmount = vaultAmount-totalChargesAndRedemptionAmount;
                    //setting amount zero to all legs
                    foreach (var amount in Merchantvault)
                    {
                        amount.Amount = 0;
                    }
                    //set remaining vault amount to first index
                    Merchantvault[0].Amount = vaultAmount;
                    
                    //Assigning to Customer
                    var res = await _gift_Service.AddReturn(giftCard);
                    if (res == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Error in adding gift card";
                        return response;
                    }

                    //update Merchant vault 
                    _vault_Service.Update(Merchantvault,x=>x.Amount);
                    
                    //customer voucher
                    Account_Transaction account_Transaction     = new();
                    IList<Account_Transaction> Transactions     = new List<Account_Transaction>();
                    //Add customer gift card credit leg
                    account_Transaction.Tenant_Id               = TenantId.ToString();
                    account_Transaction.Amount                  = giftCard.Balance;
                    account_Transaction.Card_Id                 = serialNo;
                    account_Transaction.Card_Type               = giftCard.Type;
                    account_Transaction.Customer_First_Name     = model.Customer_Name;
                    account_Transaction.Email                   = model.Customer_email;
                    account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Credit;
                    account_Transaction.Processor_Id            = userId;
                    account_Transaction.Processor_Name          = userName;
                    account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Credit;
                    
                    account_Transaction.RedemptionType          = Account_Transaction_Type_GModel.AddBalancetoCustomerGiftCard;
                    
                    Transactions.Add(account_Transaction);
                    //add Arenas Charges 
                    //Arenas Super User
                    ApplicationUser superUser       = await _authManager_Service.FindbyEmail("SuperAdmin@gmail.com");
                    response                        = CatchExceptionNull(superUser);
                    if (response.Status_Code != "200")
                    {
                        response.Status_Code = "404";
                        response.Description = "Could not find Super Admin record";
                        return response;
                    }
                    
                    ApplicationUser MerchantRecord  = await _authManager_Service.FindById(userId);
                    response                        = CatchExceptionNull(MerchantRecord);
                    if (response.Status_Code != "200")
                    {
                        response.Status_Code = "404";
                        response.Description = "Could not find Merchant record";
                        return response;
                    }
                    
                    Account_Transaction accountTransactionArenas     = new();
                    accountTransactionArenas.Tenant_Id               = superUser.TenantId.ToString();
                    accountTransactionArenas.Amount                  = charges.ChargeAmount.Value;
                    accountTransactionArenas.Card_Id                 = serialNo;
                    accountTransactionArenas.Card_Type               = giftCard.Type;
                    accountTransactionArenas.Customer_First_Name     = MerchantRecord.FirstName + " " + MerchantRecord.LastName;
                    accountTransactionArenas.Email                   = MerchantRecord.Email;
                    accountTransactionArenas.DrCrFlag                = Account_Txn_Flag_GModel.Credit;
                    accountTransactionArenas.Processor_Id            = superUser.Id;
                    accountTransactionArenas.Processor_Name          = superUser.FirstName + " " + superUser.LastName;
                    accountTransactionArenas.Txn_Type                = Account_Transaction_Type_GModel.Credit;
                    
                    accountTransactionArenas.RedemptionType          = Account_Transaction_Type_GModel.ChargesaddedtoArenasvault;
                    
                    Transactions.Add(accountTransactionArenas);
                    //Add credit in Arenas Vault
                    Vault arenasvault           = new Vault();
                    arenasvault.TenantId        = superUser.TenantId;
                    arenasvault.Amount          = charges.ChargeAmount.Value;
                    arenasvault.Email           = superUser.Email;
                    arenasvault.Lastupdated     = DateTime.UtcNow;
                    arenasvault.UserId          = superUser.Id;
                    arenasvault.VaultType       = "SuperAdmin";
                    
                    var arenasvaultresponse= await _vault_Service.AddReturn(arenasvault);
                    if (arenasvaultresponse == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Could not add charge to arenas vault";
                    }
                    
                    //Two charges legs for Merchant account one for gift card balance debit and one redemption charges debit 
                    Account_Transaction accountTransactionMerchant     = new();
                    accountTransactionMerchant.Tenant_Id               = MerchantRecord.TenantId.ToString();
                    accountTransactionMerchant.Amount                  = giftCard.Balance;
                    accountTransactionMerchant.Card_Id                 = serialNo;
                    accountTransactionMerchant.Card_Type               = giftCard.Type;
                    accountTransactionMerchant.Customer_First_Name     = model.Customer_Name;
                    accountTransactionMerchant.Email                   = model.Customer_email;
                    accountTransactionMerchant.DrCrFlag                = Account_Txn_Flag_GModel.Debit;
                    accountTransactionMerchant.Processor_Id            = MerchantRecord.Id;
                    accountTransactionMerchant.Processor_Name          = MerchantRecord.FirstName + " " + MerchantRecord.LastName;
                    accountTransactionMerchant.Txn_Type                = Account_Transaction_Type_GModel.Debit;
                    
                    accountTransactionMerchant.RedemptionType          = Account_Transaction_Type_GModel.BalanceDebitedFromMerchantVault;
                    
                    Transactions.Add(accountTransactionMerchant);
                    //Charges leg as debit
                    Account_Transaction accountTransactionMerchantCharges     = new();
                    accountTransactionMerchantCharges.Tenant_Id               = MerchantRecord.TenantId.ToString();
                    accountTransactionMerchantCharges.Amount                  = charges.ChargeAmount.Value;
                    accountTransactionMerchantCharges.Card_Id                 = serialNo;
                    accountTransactionMerchantCharges.Card_Type               = giftCard.Type;
                    accountTransactionMerchantCharges.Customer_First_Name     = MerchantRecord.FirstName + " " + MerchantRecord.LastName;
                    accountTransactionMerchantCharges.Email                   = MerchantRecord.Email;
                    accountTransactionMerchantCharges.DrCrFlag                = Account_Txn_Flag_GModel.Debit;
                    accountTransactionMerchantCharges.Processor_Id            = superUser.Id;
                    accountTransactionMerchantCharges.Processor_Name          = superUser.FirstName + " " + superUser.LastName;
                    accountTransactionMerchantCharges.Txn_Type                = Account_Transaction_Type_GModel.Debit;
                    accountTransactionMerchantCharges.RedemptionType          = Account_Transaction_Type_GModel.ChargesDebitedFromMerchantVault;
                    
                    Transactions.Add(accountTransactionMerchantCharges);
                    
                    //add transactions
                    var Transactionresponse=await _transactionService.AddBulk(Transactions);
                    if (Transactionresponse == null || Transactionresponse.Count() == 0)
                    {
                        response.Status_Code = "400";
                        response.Description = "Could not add transactions";
                        return response;
                    }
                }


                // else if (walletPass.Type== Pass_Type_GModel.MembershipCard)
                // {
                //     //Fing Gift card
                //     MembershipCard   giftCard  = await _membership_Card_Service.FindOne(x => x.Id == model.Card_Id);
                //     giftCard.Recipient_Name    = model.Customer_Name;
                //     giftCard.Email             = model.Customer_email;
                //     giftCard.Phone             = model.Phone;
                //     giftCard.Sender_Name       = model.Store_Name ?? tenantName;
                //     giftCard.Address           = model.Store_Address;
                //     giftCard.Pass_Status       = Pass_Redemption_Status_GModel.Redeemable;
                //     giftCard.Serial_Number     = serialNo.ToString();
                //     
                //     giftCard.Id                = 0;
                //     
                //     //Assigning to Customer
                //     var res = await _membership_Card_Service.AddReturn(giftCard);
                //     
                //     if (res == null)
                //     {
                //         response.Status_Code = "404";
                //         response.Description = "Error in adding gift card";
                //         return response;
                //     }
                //
                //     //Add Customer Account 
                //     account_Balance.ACCOUNT_NO                  = serialNo.ToString();
                //     account_Balance.Customer_Email              = model.Customer_email;
                //     account_Balance.Customer_FName              = model.Customer_Name;
                //     account_Balance.Customer_LName              = model.Customer_Name;
                //     account_Balance.Account_Status              = AccountStatus.PendingForSend;
                //     account_Balance.Tenant_Id                   = TenantId;
                //     account_Balance.Account_Type                = giftCard.Type;
                //     account_Balance.Amount                      = 0;
                //     account_Balance.Processor                   = userName;
                //     account_Balance.Process_By                  = tenantName;
                //     
                //     //Add customer gift card credit leg
                //     account_Transaction.Tenant_Id               = TenantId.ToString();
                //     account_Transaction.Amount                  = 0;
                //     account_Transaction.Card_Id                 = serialNo;
                //     account_Transaction.Card_Type               = giftCard.Type;
                //     account_Transaction.Customer_First_Name           = model.Customer_Name;
                //    
                //     account_Transaction.Email                   = model.Customer_email;
                //     account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Non_Financial;
                //     account_Transaction.Processor_Id            = userId;
                //     account_Transaction.Processor_Name          = userName;
                //     account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Non_Financial;
                // }
                // else if (walletPass.Type== Pass_Type_GModel.LoyaltyCard)
                // {
                //     //Fing Gift card
                //     LoyaltyCard   giftCard     = await _loyalty_Card_Service.FindOne(x => x.Id == model.Card_Id);
                //     giftCard.Recipient_Name    = model.Customer_Name;
                //     giftCard.Email             = model.Customer_email;
                //     giftCard.Phone             = model.Phone;
                //     giftCard.Sender_Name       = model.Store_Name ?? tenantName;
                //     giftCard.Address           = model.Store_Address;
                //     giftCard.Pass_Status       = Pass_Redemption_Status_GModel.Redeemable;
                //     giftCard.Serial_Number     = serialNo.ToString();
                //     giftCard.Id                = 0;
                //     
                //     //Assigning to Customer
                //     var res = await _loyalty_Card_Service.AddReturn(giftCard);
                //     
                //     if (res == null)
                //     {
                //         response.Status_Code = "404";
                //         response.Description = "Error in adding gift card";
                //         return response;
                //     }
                //
                //     //Add Customer Account 
                //     
                //     //Add customer gift card credit leg
                //     account_Transaction.Tenant_Id               = TenantId.ToString();
                //     account_Transaction.Amount                  = 0;
                //     account_Transaction.Card_Id                 = serialNo;
                //     account_Transaction.Card_Type               = giftCard.Type;
                //     account_Transaction.Customer_First_Name     = model.Customer_Name;
                //     
                //     account_Transaction.Email                   = model.Customer_email;
                //     account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Non_Financial;
                //     account_Transaction.Processor_Id            = userId;
                //     account_Transaction.Processor_Name          = userName;
                //     account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Non_Financial;
                // }
                // else if (walletPass.Type== Pass_Type_GModel.PunchCard)
                // {
                //     //Fing Gift card
                //     PunchCard   giftCard     = await _punch_Card_Service.FindOne(x => x.Id == model.Card_Id);
                //     giftCard.Recipient_Name    = model.Customer_Name;
                //     giftCard.Email             = model.Customer_email;
                //     giftCard.Phone             = model.Phone;
                //     giftCard.Sender_Name       = model.Store_Name ?? tenantName;
                //     giftCard.Address           = model.Store_Address;
                //     giftCard.Pass_Status       = Pass_Redemption_Status_GModel.Redeemable;
                //     giftCard.Serial_Number     = serialNo.ToString();
                //     
                //     giftCard.Id                = 0;
                //     
                //     //Assigning to Customer
                //     var res = await _punch_Card_Service.AddReturn(giftCard);
                //     
                //     if (res == null)
                //     {
                //         response.Status_Code = "404";
                //         response.Description = "Error in adding gift card";
                //         return response;
                //     }
                //
                //     //Add Customer Account 
                //     account_Balance.ACCOUNT_NO                  = serialNo.ToString();
                //     account_Balance.Customer_Email              = model.Customer_email;
                //     account_Balance.Customer_FName              = model.Customer_Name;
                //     account_Balance.Customer_LName              = model.Customer_Name;
                //     account_Balance.Account_Status              = AccountStatus.PendingForSend;
                //     account_Balance.Tenant_Id                   = TenantId;
                //     account_Balance.Account_Type                = giftCard.Type;
                //     account_Balance.Amount                      = 0;
                //     account_Balance.Processor                   = userName;
                //     account_Balance.Process_By                  = tenantName;
                //     
                //     //Add customer gift card credit leg
                //     account_Transaction.Tenant_Id               = TenantId.ToString();
                //     account_Transaction.Amount                  = 0;
                //     account_Transaction.Card_Id                 = serialNo;
                //     account_Transaction.Card_Type               = giftCard.Type;
                //     account_Transaction.Customer_First_Name     = model.Customer_Name;
                //     
                //     account_Transaction.Email                   = model.Customer_email;
                //     account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Non_Financial;
                //     account_Transaction.Processor_Id            = userId;
                //     account_Transaction.Processor_Name          = userName;
                //     account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Non_Financial;
                // }
                
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
