using Server.UOW;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Gift_Card_Service:Base_Service<GiftCard> ,IGift_Card_Service
    {
        #region Constructor
        private readonly IApple_Passes_Service    _applePasses_Service;
        private readonly ITransaction_No_Service  tnService;
        private readonly IGift_Cards_Repo        _iRepo;
        private readonly IGet_Tenant_Id_Service _contextAccessor;

        public Gift_Card_Service
        (
            IApple_Passes_Service    applePasses_Service,
            ITransaction_No_Service  tn,
            IUnitOfWork              unitOfWork,
            IGift_Cards_Repo         iRepo,
            IGet_Tenant_Id_Service contextAccessor

        ) :base(unitOfWork,iRepo)
        {
            _applePasses_Service = applePasses_Service;
            tnService            = tn;
            _iRepo               = iRepo;
            _contextAccessor     = contextAccessor;
        }

        public async Task<ResponseModel<string>> DeleteGiftCard(int GiftCardId, int tenantId)
        {
            try
            {
                ResponseModel<string> giftResponse = new()
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };
            
                giftResponse.Description= await _iRepo.DeletRange(x=>x.TenantId==tenantId && x.Id==GiftCardId);
                 
                if( giftResponse.Description!="OK")
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Response    = "Error in Deletion";
                }

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
                int serialNo              = await tnService.GetTxnNo();
                //Getting Pass
                
                GiftCard gift             = new();
                gift.Type                 = "GiftCard"; 
                gift.Apple_Pass           = (byte[])giftResponse.Response;
                gift.Background_Color     = GiftCard.Background_Color;
                gift.Label_Color          = GiftCard.Label_Color;
                gift.Foreground_Color     = GiftCard.Foreground_Color;
                gift.Localized_Name       = GiftCard.Logo_Text;
                gift.Terms_And_Conditions = GiftCard.Terms_And_Conditions;
                gift.Privacy_Policy       = GiftCard.Privacy_Policy;
                gift.Logo_Url             = GiftCard.Logo_Url;
                gift.Logo_Text            = GiftCard.Logo_Text;
                gift.Organization_Name    = "ArenasPass";
                gift.Serial_Number        = serialNo.ToString();
                gift.Description          = string.IsNullOrEmpty(GiftCard.Description)?"N/A": GiftCard.Description;
                gift.Web_Service_URL      = GiftCard.Webiste;
                gift.Authentication_Token = "";
                
                // Set properties specific to GiftCard
                gift.Currency_Code        = GiftCard.Currency_Code;
                gift.Recipient_Name       = GiftCard.Recipient_Name;
                gift.Sender_Name          = GiftCard.Sender_Name;
                gift.Message              = GiftCard.Message;  
                gift.Code_Type            = GiftCard.Code_Type;
                gift.Barcode_Format       = giftResponse.Description;
                gift.Expiration_Date      = GiftCard.Expiration_Date;
                gift.Relevant_Date        = GiftCard.Relevant_Date;
                gift.TenantId             = _contextAccessor.GetTenantId();
                gift.Balance              = !string.IsNullOrEmpty(GiftCard.Balance)? decimal.Parse(GiftCard.Balance):0;

                gift.Card_Holder_Title    = GiftCard.Card_Holder_Title;
                gift.Card_holder_Name     = GiftCard.Card_holder_Name;
                gift.Effective_Date       = GiftCard.Effective_Date;
                gift.Issuer               = GiftCard.Issuer;
                gift.Email                = GiftCard.Email;
                gift.Phone                = GiftCard.Phone;
                gift.Address              = GiftCard.Address;
                gift.Currency_Sign        = GiftCard.Currency_Sign;

                await _iRepo.Add(gift);
                await _iRepo.Commit();
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

        /// <summary>
        /// Update Gift Card
        /// </summary>
        /// <param name="GiftCard"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseModel<string>> UpdateGiftCard(Apple_Passes_Gift_Card_Model GiftCard)
        {
           try
            {
                ResponseModel<string> giftResponse = new()
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

             
                GiftCard gift             = await _iRepo.FindOne(x=>x.Id==GiftCard.Id);
                if (gift == null) { giftResponse.Status_Code = "404"; giftResponse.Description = "No reocrd found"; return giftResponse; }

                gift.Apple_Pass           = giftResponse.Response != null ? (byte[])giftResponse.Response : gift.Apple_Pass;
                gift.Background_Color     = !string.IsNullOrEmpty(GiftCard.Background_Color) ? GiftCard.Background_Color : gift.Background_Color;
                gift.Label_Color          = !string.IsNullOrEmpty(GiftCard.Label_Color) ? GiftCard.Label_Color : gift.Label_Color;
                gift.Foreground_Color     = !string.IsNullOrEmpty(GiftCard.Foreground_Color) ? GiftCard.Foreground_Color : gift.Foreground_Color;
                gift.Localized_Name       = !string.IsNullOrEmpty(GiftCard.Logo_Text) ? GiftCard.Logo_Text : gift.Localized_Name;
                gift.Terms_And_Conditions = !string.IsNullOrEmpty(GiftCard.Terms_And_Conditions) ? GiftCard.Terms_And_Conditions : gift.Terms_And_Conditions;
                gift.Privacy_Policy       = !string.IsNullOrEmpty(GiftCard.Privacy_Policy) ? GiftCard.Privacy_Policy : gift.Privacy_Policy;
                gift.Logo_Url             = !string.IsNullOrEmpty(GiftCard.Logo_Url) ? GiftCard.Logo_Url : gift.Logo_Url;
                gift.Logo_Text            = !string.IsNullOrEmpty(GiftCard.Logo_Text) ? GiftCard.Logo_Text : gift.Logo_Text;
                gift.Organization_Name    = "ArenasPass"; // Always set this property
                gift.Description          = !string.IsNullOrEmpty(GiftCard.Description) ? GiftCard.Description : gift.Description;
                gift.Web_Service_URL      = !string.IsNullOrEmpty(GiftCard.Webiste) ? GiftCard.Webiste : gift.Web_Service_URL;
                gift.Authentication_Token = gift.Authentication_Token ?? ""; // Ensure a default value

                gift.Currency_Code        = !string.IsNullOrEmpty(GiftCard.Currency_Code) ? GiftCard.Currency_Code : gift.Currency_Code;
                gift.Recipient_Name       = !string.IsNullOrEmpty(GiftCard.Recipient_Name) ? GiftCard.Recipient_Name : gift.Recipient_Name;
                gift.Sender_Name          = !string.IsNullOrEmpty(GiftCard.Sender_Name) ? GiftCard.Sender_Name : gift.Sender_Name;
                gift.Message              = !string.IsNullOrEmpty(GiftCard.Message) ? GiftCard.Message : gift.Message;
                gift.Code_Type            = !string.IsNullOrEmpty(GiftCard.Code_Type) ? GiftCard.Code_Type : gift.Code_Type;
                gift.Barcode_Format       = !string.IsNullOrEmpty(giftResponse.Description) ? giftResponse.Description : gift.Barcode_Format;
                gift.Expiration_Date      = GiftCard.Expiration_Date ?? gift.Expiration_Date;
                gift.Relevant_Date        = GiftCard.Relevant_Date ?? gift.Relevant_Date;
                gift.Balance              = !string.IsNullOrEmpty(GiftCard.Balance) ? decimal.Parse(GiftCard.Balance) : gift.Balance;

                gift.Card_Holder_Title    = !string.IsNullOrEmpty(GiftCard.Card_Holder_Title) ? GiftCard.Card_Holder_Title : gift.Card_Holder_Title;
                gift.Card_holder_Name     = !string.IsNullOrEmpty(GiftCard.Card_holder_Name) ? GiftCard.Card_holder_Name : gift.Card_holder_Name;
                gift.Effective_Date       = GiftCard.Effective_Date ?? gift.Effective_Date;
                gift.Issuer               = !string.IsNullOrEmpty(GiftCard.Issuer) ? GiftCard.Issuer : gift.Issuer;
                gift.Email                = !string.IsNullOrEmpty(GiftCard.Email) ? GiftCard.Email : gift.Email;
                gift.Phone                = !string.IsNullOrEmpty(GiftCard.Phone) ? GiftCard.Phone : gift.Phone;
                gift.Address              = !string.IsNullOrEmpty(GiftCard.Address) ? GiftCard.Address : gift.Address;
                gift.Currency_Sign        = gift.Currency_Code=="Dollar"? "$": "€";
                gift.Webiste              = !string.IsNullOrEmpty(GiftCard.Webiste)? GiftCard.Webiste:gift.Webiste;

                _iRepo.Update(gift);
                await _iRepo.Save();

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

        public ResponseModel<string> ValidateGiftCardForRedemption(GiftCard GiftCard)
        {
            try
            {
                ResponseModel<string> RedeemResponse = new ResponseModel<string>() { Status_Code = "200", Description = "OK" };
                if (GiftCard.Pass_Status == "Template")
                {
                    RedeemResponse.Status_Code = "400";
                    RedeemResponse.Description = "Error the Gift card is currently Template cannot be Redeemable. Kindly send this card to any customer first by providing the customer detail, after that it can be redeemable";
                    return RedeemResponse;
                }
                else if (GiftCard.Pass_Status == "Redeemed")
                {
                    RedeemResponse.Status_Code = "400";
                    RedeemResponse.Description = "Error the Gift card is already Redeemed";
                    return RedeemResponse;
                }
                else if (GiftCard.Pass_Status != "Redeemable" || GiftCard.Pass_Status != "Partial-Redeemable")
                {
                    RedeemResponse.Status_Code = "400";
                    RedeemResponse.Description = "Error the Gift card current status is not Redeemable.Kindly re-assign to customer";
                    return RedeemResponse;
                }
                return RedeemResponse;
            }
            catch (Exception ex)
            {

                return CatchException(ex);
            }
        }
    }
}
