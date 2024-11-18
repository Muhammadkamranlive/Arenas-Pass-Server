using Server.Core;
using Server.Domain;
using Server.Models;

namespace Server.Services
{
    public class Gift_Card_Service:IGift_Card_Service
    {
        #region Constructor
        private readonly IApple_Passes_Service _applePasses_Service;
        private readonly ERPDb                 _db;
        public Gift_Card_Service
        (
            IApple_Passes_Service applePasses_Service,
            ERPDb db
        ) 
        {
            _applePasses_Service = applePasses_Service;
            _db                  = db;
        }

        #endregion


        /// <summary>
        /// Generate Gift Cards
        /// </summary>
        /// <param name="GiftCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateGiftCard(Apple_Passes_Gift_Card_Model GiftCard)
        {
            try
            {
                ResponseModel<string> giftResponse = new()
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                //Getting Pass
                giftResponse= await _applePasses_Service.GiftCards( GiftCard );
                if (giftResponse.Status_Code != "200") { return giftResponse; }
                GiftCard gift             = new();
                gift.Type                 = "GiftCard"; 
                gift.Apple_Pass           = (byte[])giftResponse.Response;
                gift.Background_Color     = GiftCard.Background_Color;
                gift.Label_Color          = GiftCard.Label_Color;
                gift.Foreground_Color     = GiftCard.Foreground_Color;
                gift.Localized_Name       = GiftCard.Logo_Text;
                gift.Terms_And_Conditions = GiftCard.Privacy_Policy;
                gift.Logo                 = GiftCard.Logo_Url;
                gift.Organization_Name    = GiftCard.Organization_Name;
                gift.Serial_Number        = GiftCard.Serial_Number;
                gift.Description          = GiftCard.Description;
                gift.Web_Service_URL      = GiftCard.Webiste;
                gift.Authentication_Token = "";
                
                // Set properties specific to GiftCard
                gift.Currency_Code        = GiftCard.Currency_Code;
                gift.Recipient_Name       = GiftCard.Card_holder_Name;
                gift.Sender_Name          = GiftCard.Sender_Name;
                gift.Message              = GiftCard.Notes;  
                gift.Barcode_Type         = GiftCard.Code_Type;
                gift.Barcode_Format       = giftResponse.Description;
                gift.Expiration_Date      = GiftCard.Expiry_Date;
                gift.Relevant_Date        = GiftCard.Expiry_Date;
                gift.Balance              = !string.IsNullOrEmpty(GiftCard.Balance)? decimal.Parse(GiftCard.Balance):0;

                _db.WalletPasses.Add(gift);
                await _db.SaveChangesAsync();
                //Saving the Gift Card
                return giftResponse;
            }
            catch (Exception ex)
            {
                ResponseModel<string> giftResponse = new ResponseModel<string>()
                {
                    Status_Code = "500",
                    Description = ex.Message,
                    Response    = "Error Occurred"
                };
                return giftResponse;
            }
        }


    }
}
