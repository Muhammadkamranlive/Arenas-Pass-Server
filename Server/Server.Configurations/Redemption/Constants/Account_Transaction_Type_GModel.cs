using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Configurations
{
    public static class Account_Transaction_Type_GModel
    {
        public const string Credit        = "Credit";
        public const string Debit         = "Debit";
        public const string Non_Financial = "NonFinancial";
        public const string AddetoVault   = "Credit balance in Vault";
        public const  string withdraw     = "Debit balance from Vault";
        public const string AddBalancetoCustomerGiftCard = "Balance is added to Customer Gift Card";
        public const string ChargesaddedtoArenasvault    = "Charges are added to Arenas vault";
        public const string BalanceDebitedFromMerchantVault = "Balance debited from Merchant's vault";
        public const string ChargesDebitedFromMerchantVault = "Charges debited from Merchant's vault";
        public const string DebitedBalanceFromGiftCard= "Debited balance from Gift Card";
        public const string CreditInMerchantVault = "Credit balance in Merchant Vault";
        public const string CreditAddedinCustomerVault = "Credit balance in Customer Vault";
    }
}
