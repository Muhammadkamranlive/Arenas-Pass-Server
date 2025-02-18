namespace Server.Configurations;


public static class ChargesTypeEnum
{
    public static class MerchantCharges
    {
        public const string GiftCardIssuance = "gift_card_issuance";
        public const string GiftCardRedemption = "gift_card_redemption";
        public const string Subscription = "subscription";
        public const string ApiAccess = "api_access";
        public const string TransactionProcessing = "transaction_processing";
        public const string Chargeback = "chargeback";
        public const string CustomBranding = "custom_branding";
        public const string BulkIssuanceDiscount = "bulk_issuance_discount";
    }
    
    public static class CustomerCharges
    {
        public const string GiftCardPurchase = "gift_card_purchase";
        public const string Reload = "reload";
        public const string InactiveCard = "inactive_card";
        public const string CardReplacement = "card_replacement";
    }
    
    public static class APICharges
    {
        public const string APITransaction = "api_transaction";
        public const string MonthlyAPISubscription = "monthly_api_subscription";
        public const string HighVolumeAPI = "high_volume_api";
    }
}
