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
        private readonly IAuthManager                 _authManager_Service;
        private readonly IAccount_Balance_Service     _account_Balance_Service;
        private readonly IWallet_Pass_Service         _wallet_Pass_Service;
        private readonly IMembership_Card_Service     _membership_Card_Service;
        private readonly ILoyalty_Card_Service        _loyalty_Card_Service;
        private readonly IPunch_Card_Service          _punch_Card_Service;
        public Assign_Pass_Customer_Service
        (   IUnitOfWork                  unitOfWork,
            IAccount_Transaction_Repo    genericRepository,
            IAccount_Transaction_Service account_Transaction,
            IGet_Tenant_Id_Service       get_Tenant_Id,
            ITransaction_No_Service      transaction_No,
            IGift_Card_Service           gift_Card,
            IAuthManager                 authManager,
            IWallet_Pass_Service         wallet_Pass,
            IAccount_Balance_Service     account_Balance,
            IMembership_Card_Service     membership_Card,
            ILoyalty_Card_Service        loyalty_Card,
            IPunch_Card_Service          punch_Card

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
                //Find User
                if (model.Customer_Type == "Registered")
                {
                    ApplicationUser user = await _authManager_Service.FindbyEmail(model.Customer_email);
                    response = CatchExceptionNull(user);
                    if (response.Status_Code != "200")
                    {
                        response.Description = response.Description + "Oops looks like customer is not registered";
                        return response;
                    }
                    model.Customer_email       = user.Email;
                    model.Customer_Name        = user.FirstName + " "+ user.LastName;
                    model.Phone                = user.PhoneNumber;
                    model.Customer_Type        = "Registered";
                }

                //find tenant
                var tenantName                 = _Tenant_Id_Service.GetCompanyName();
                var TenantId                   = _Tenant_Id_Service.GetTenantId();
                var userId                     = _Tenant_Id_Service.GetUserId();
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
                Account_Transaction account_Transaction = new();
                Account_Balance account_Balance         = new();


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
                    giftCard.Pass_Status      = Pass_Redemption_Status_GModel.Redeemable;
                    giftCard.Serial_Number    = serialNo.ToString();
                    
                    giftCard.Id               = 0;
                    
                    //Assigning to Customer
                    var res = await _gift_Service.AddReturn(giftCard);
                    
                    if (res == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Error in adding gift card";
                        return response;
                    }

                    //Add Customer Account 
                    account_Balance.ACCOUNT_NO                  = serialNo.ToString();
                    account_Balance.Customer_Email              = model.Customer_email;
                    account_Balance.Customer_FName              = model.Customer_Name;
                    account_Balance.Customer_LName              = model.Customer_Name;
                    account_Balance.Account_Status              = AccountStatus.PendingForSend;
                    account_Balance.Tenant_Id                   = TenantId;
                    account_Balance.Account_Type                = giftCard.Type;
                    account_Balance.Amount                      = giftCard.Balance;
                    account_Balance.Processor                   = userName;
                    account_Balance.Process_By                  = tenantName;
                    
                    //Add customer gift card credit leg
                    account_Transaction.Tenant_Id               = TenantId.ToString();
                    account_Transaction.Amount                  = giftCard.Balance;
                    account_Transaction.Card_Id                 = serialNo;
                    account_Transaction.Card_Type               = giftCard.Type;
                    account_Transaction.Customer_First_Name     = model.Customer_Name;
                    account_Transaction.Customer_Last_Name      = model.Customer_Name;
                    account_Transaction.Email                   = model.Customer_email;
                    account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Credit;
                    account_Transaction.Processor_Id            = userId;
                    account_Transaction.Processor_Name          = userName;
                    account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Credit;
                }


                else if (walletPass.Type== Pass_Type_GModel.MembershipCard)
                {
                    //Fing Gift card
                    MembershipCard   giftCard  = await _membership_Card_Service.FindOne(x => x.Id == model.Card_Id);
                    giftCard.Recipient_Name    = model.Customer_Name;
                    giftCard.Email             = model.Customer_email;
                    giftCard.Phone             = model.Phone;
                    giftCard.Sender_Name       = model.Store_Name ?? tenantName;
                    giftCard.Address           = model.Store_Address;
                    giftCard.Pass_Status       = Pass_Redemption_Status_GModel.Redeemable;
                    giftCard.Serial_Number     = serialNo.ToString();
                    
                    giftCard.Id                = 0;
                    
                    //Assigning to Customer
                    var res = await _membership_Card_Service.AddReturn(giftCard);
                    
                    if (res == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Error in adding gift card";
                        return response;
                    }

                    //Add Customer Account 
                    account_Balance.ACCOUNT_NO                  = serialNo.ToString();
                    account_Balance.Customer_Email              = model.Customer_email;
                    account_Balance.Customer_FName              = model.Customer_Name;
                    account_Balance.Customer_LName              = model.Customer_Name;
                    account_Balance.Account_Status              = AccountStatus.PendingForSend;
                    account_Balance.Tenant_Id                   = TenantId;
                    account_Balance.Account_Type                = giftCard.Type;
                    account_Balance.Amount                      = 0;
                    account_Balance.Processor                   = userName;
                    account_Balance.Process_By                  = tenantName;
                    
                    //Add customer gift card credit leg
                    account_Transaction.Tenant_Id               = TenantId.ToString();
                    account_Transaction.Amount                  = 0;
                    account_Transaction.Card_Id                 = serialNo;
                    account_Transaction.Card_Type               = giftCard.Type;
                    account_Transaction.Customer_First_Name     = model.Customer_Name;
                    account_Transaction.Customer_Last_Name      = model.Customer_Name;
                    account_Transaction.Email                   = model.Customer_email;
                    account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Non_Financial;
                    account_Transaction.Processor_Id            = userId;
                    account_Transaction.Processor_Name          = userName;
                    account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Non_Financial;
                }


                else if (walletPass.Type== Pass_Type_GModel.LoyaltyCard)
                {
                    //Fing Gift card
                    LoyaltyCard   giftCard     = await _loyalty_Card_Service.FindOne(x => x.Id == model.Card_Id);
                    giftCard.Recipient_Name    = model.Customer_Name;
                    giftCard.Email             = model.Customer_email;
                    giftCard.Phone             = model.Phone;
                    giftCard.Sender_Name       = model.Store_Name ?? tenantName;
                    giftCard.Address           = model.Store_Address;
                    giftCard.Pass_Status       = Pass_Redemption_Status_GModel.Redeemable;
                    giftCard.Serial_Number     = serialNo.ToString();
                    
                    giftCard.Id                = 0;
                    
                    //Assigning to Customer
                    var res = await _loyalty_Card_Service.AddReturn(giftCard);
                    
                    if (res == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Error in adding gift card";
                        return response;
                    }

                    //Add Customer Account 
                    account_Balance.ACCOUNT_NO                  = serialNo.ToString();
                    account_Balance.Customer_Email              = model.Customer_email;
                    account_Balance.Customer_FName              = model.Customer_Name;
                    account_Balance.Customer_LName              = model.Customer_Name;
                    account_Balance.Account_Status              = AccountStatus.PendingForSend;
                    account_Balance.Tenant_Id                   = TenantId;
                    account_Balance.Account_Type                = giftCard.Type;
                    account_Balance.Amount                      = 0;
                    account_Balance.Processor                   = userName;
                    account_Balance.Process_By                  = tenantName;
                    
                    //Add customer gift card credit leg
                    account_Transaction.Tenant_Id               = TenantId.ToString();
                    account_Transaction.Amount                  = 0;
                    account_Transaction.Card_Id                 = serialNo;
                    account_Transaction.Card_Type               = giftCard.Type;
                    account_Transaction.Customer_First_Name     = model.Customer_Name;
                    account_Transaction.Customer_Last_Name      = model.Customer_Name;
                    account_Transaction.Email                   = model.Customer_email;
                    account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Non_Financial;
                    account_Transaction.Processor_Id            = userId;
                    account_Transaction.Processor_Name          = userName;
                    account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Non_Financial;
                }


                else if (walletPass.Type== Pass_Type_GModel.PunchCard)
                {
                    //Fing Gift card
                    PunchCard   giftCard     = await _punch_Card_Service.FindOne(x => x.Id == model.Card_Id);
                    giftCard.Recipient_Name    = model.Customer_Name;
                    giftCard.Email             = model.Customer_email;
                    giftCard.Phone             = model.Phone;
                    giftCard.Sender_Name       = model.Store_Name ?? tenantName;
                    giftCard.Address           = model.Store_Address;
                    giftCard.Pass_Status       = Pass_Redemption_Status_GModel.Redeemable;
                    giftCard.Serial_Number     = serialNo.ToString();
                    
                    giftCard.Id                = 0;
                    
                    //Assigning to Customer
                    var res = await _punch_Card_Service.AddReturn(giftCard);
                    
                    if (res == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Error in adding gift card";
                        return response;
                    }

                    //Add Customer Account 
                    account_Balance.ACCOUNT_NO                  = serialNo.ToString();
                    account_Balance.Customer_Email              = model.Customer_email;
                    account_Balance.Customer_FName              = model.Customer_Name;
                    account_Balance.Customer_LName              = model.Customer_Name;
                    account_Balance.Account_Status              = AccountStatus.PendingForSend;
                    account_Balance.Tenant_Id                   = TenantId;
                    account_Balance.Account_Type                = giftCard.Type;
                    account_Balance.Amount                      = 0;
                    account_Balance.Processor                   = userName;
                    account_Balance.Process_By                  = tenantName;
                    
                    //Add customer gift card credit leg
                    account_Transaction.Tenant_Id               = TenantId.ToString();
                    account_Transaction.Amount                  = 0;
                    account_Transaction.Card_Id                 = serialNo;
                    account_Transaction.Card_Type               = giftCard.Type;
                    account_Transaction.Customer_First_Name     = model.Customer_Name;
                    account_Transaction.Customer_Last_Name      = model.Customer_Name;
                    account_Transaction.Email                   = model.Customer_email;
                    account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Non_Financial;
                    account_Transaction.Processor_Id            = userId;
                    account_Transaction.Processor_Name          = userName;
                    account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.Non_Financial;
                }

                //saving Account
                if(account_Balance != null)
                {
                    var account = await _account_Balance_Service.AddReturn(account_Balance);
                                  
                    if (account == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Error in adding account detail for customer card";
                        return response;
                    }
                }

                //saving Transaction
                if(account_Transaction!=null)
                {
                    var trans = await _transactionService.AddReturn(account_Transaction);
                   
                    if (trans == null)
                    {
                        response.Status_Code = "404";
                        response.Description = "Error in adding gift card";
                        return response;
                    }
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
