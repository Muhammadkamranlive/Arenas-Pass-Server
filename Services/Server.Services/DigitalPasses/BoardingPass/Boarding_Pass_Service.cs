using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Boarding_Pass_Service : Base_Service<BoardingPass>, IBoarding_Pass_Service
    {
        #region Constructor
        private readonly IApple_Passes_Service   _applePasses_Service;
        private readonly ITransaction_No_Service tnService;
        private readonly IBoarding_Pass_Repo    _iRepo;
        private readonly IGet_Tenant_Id_Service _contextAccessor;

        public Boarding_Pass_Service
        (
            IApple_Passes_Service applePasses_Service,
            ITransaction_No_Service tn,
            IUnitOfWork unitOfWork,
            IBoarding_Pass_Repo iRepo,
            IGet_Tenant_Id_Service contextAccessor

        ) : base(unitOfWork, iRepo)
        {
            _applePasses_Service = applePasses_Service;
            tnService            = tn;
            _iRepo               = iRepo;
            _contextAccessor     = contextAccessor;
        }

        #endregion

        /// <summary>
        /// Generate Gift Cards
        /// </summary>
        /// <param name="GiftCard"></param>
        /// <returns></returns>
        //public async Task<ResponseModel<string>> GenerateCard(Apple_Passes_Gift_Card_Model GiftCard)
        //{
        //    try
        //    {
        //        ResponseModel<string> giftResponse = new()
        //        {
        //            Status_Code = "200",
        //            Description = "OK",
        //            Response    = null
        //        };
        //        int serialNo              = await tnService.GetTxnNo();
        //        //Getting Pass
        //        giftResponse              = await _applePasses_Service.( GiftCard,serialNo.ToString());
        //        if (giftResponse.Status_Code != "200") { return giftResponse; }
                
        //        GiftCard gift             = new();
        //        gift.Type                 = "GiftCard"; 
        //        gift.Apple_Pass           = (byte[])giftResponse.Response;
        //        gift.Background_Color     = GiftCard.Background_Color;
        //        gift.Label_Color          = GiftCard.Label_Color;
        //        gift.Foreground_Color     = GiftCard.Foreground_Color;
        //        gift.Localized_Name       = GiftCard.Logo_Text;
        //        gift.Terms_And_Conditions = GiftCard.Privacy_Policy;
        //        gift.Logo                 = GiftCard.Logo_Url;
        //        gift.Organization_Name    = "ArenasPass";
        //        gift.Serial_Number        = serialNo.ToString();
        //        gift.Description          = string.IsNullOrEmpty(GiftCard.Description)?"N/A": GiftCard.Description;
        //        gift.Web_Service_URL      = GiftCard.Webiste;
        //        gift.Authentication_Token = "";
                
        //        // Set properties specific to GiftCard
        //        gift.Currency_Code        = GiftCard.Currency_Code;
        //        gift.Recipient_Name       = GiftCard.Card_holder_Name;
        //        gift.Sender_Name          = GiftCard.Sender_Name;
        //        gift.Message              = GiftCard.Notes;  
        //        gift.Barcode_Type         = GiftCard.Code_Type;
        //        gift.Barcode_Format       = giftResponse.Description;
        //        gift.Expiration_Date      = GiftCard.Expiry_Date;
        //        gift.Relevant_Date        = GiftCard.Expiry_Date;
        //        gift.TenantId             = _contextAccessor.GetTenantId();
        //        gift.Balance              = !string.IsNullOrEmpty(GiftCard.Balance)? decimal.Parse(GiftCard.Balance):0;

        //        await _iRepo.Add(gift);
        //        await _iRepo.Commit();
        //        //Saving the Gift Card
        //        return giftResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        ResponseModel<string> giftResponse = new ResponseModel<string>()
        //        {
        //            Status_Code = "500",
        //            Description = ex.Message,
        //            Response    = "Error Occurred"
        //        };
        //        return giftResponse;
        //    }
        //}





    }
}
