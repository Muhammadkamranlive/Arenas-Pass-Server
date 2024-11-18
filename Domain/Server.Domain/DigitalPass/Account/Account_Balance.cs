using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{  
    public class Account_Balance
    {
        public int Id                           { get; set; }
        public string TenantId                  { get; set; }
        public string ACCOUNT_NO                { get; set; }
        public string PRODUCT_CODE              { get; set; }
        public string CURRENCY_CODE             { get; set; } = "";
        public string SHORT_TITLE               { get; set; }
        public string ACCOUNT_TITLE             { get; set; }
        public string RELATION_ID               { get; set; }
        public byte ID_LEVEL                    { get; set; } = 1;
        public string ID_IDENTIFIER             { get; set; }
        public DateTime OPEN_DATE               { get; set; }
        public string ACCOUNT_STATUS_CODE       { get; set; } = "00";
        public DateTime? STATUS_DATE            { get; set; }
        public decimal CLOSING_BALANCE          { get; set; } = 0;
        public decimal TODAYS_CR                { get; set; } = 0;
        public decimal TODAYS_DR                { get; set; } = 0;
        public decimal LOCAL_EQV                { get; set; } = 0;
        public decimal FORWARD_CR               { get; set; } = 0;
        public decimal FORWARD_DR               { get; set; } = 0;
        public decimal FORWARD_CRLOCEQV         { get; set; } = 0;
        public decimal FORWARD_DRLOCEQV         { get; set; } = 0;
        public DateTime? LAST_TRANS_DATE        { get; set; }
        public DateTime? LAST_DR_TXN_DATE       { get; set; }
        public DateTime? LAST_CR_TXN_DATE       { get; set; }
        public DateTime? LAST_OD_DATE           { get; set; }
        public decimal AUTOVCHCR                { get; set; } = 0;
        public decimal AUTOVCHDR                { get; set; } = 0;
        public DateTime? LAST_PAY_CAP_DATE      { get; set; }
        public DateTime? LAST_CHARGE_CAP_DATE   { get; set; }
        public DateTime? EXCESS_OVER_LIMIT_DATE { get; set; }
        public char? SPECIAL_RATE               { get; set; } = 'N';
        public string LEGAL_CODE                { get; set; }
        public string MOBILE_NUMBER             { get; set; }
        public string EMAIL_ADDRESS             { get; set; }
        public string VERIFIED_BY               { get; set; }
        public DateTime? VERIFICATION_DATE      { get; set; }
        public string REMARKS                   { get; set; }
        public string ACCOUNT_OP_CODE           { get; set; }
        public string SESSION_ID                { get; set; }
    }
}
