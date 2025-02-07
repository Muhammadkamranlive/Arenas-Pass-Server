using System;
using Server.UOW;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using Server.Configurations;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public class Punch_Card_Service : Base_Service<PunchCard>, IPunch_Card_Service
    {
       
        #region Constructor
        private readonly IApple_Passes_Service    _applePasses_Service;
        private readonly ITransaction_No_Service  tnService;
        private readonly IPunch_Card_Repo         _iRepo;
        private readonly IGet_Tenant_Id_Service   _contextAccessor;

        public Punch_Card_Service
        (
            IApple_Passes_Service    applePasses_Service,
            ITransaction_No_Service  tn,
            IUnit_Of_Work_Repo unitOfWork,
            IPunch_Card_Repo iRepo,
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
            
                giftResponse.Description= await _iRepo.DeletRange(x=>x.TenantId==tenantId && x.Id==GiftCardId && x.Pass_Status==Pass_Redemption_Status_GModel.Template);
                 
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
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateCard(Apple_Passes_Punch_Card_Model model)
        {
            try
            {

                //Transaction Service
                ResponseModel<string> giftResponse = new()
                {
                    Status_Code                    = "200",
                    Description                    = "OK",
                    Response                       = null
                };

                //Get Txn No
                int serialNo                     = await tnService.GetTxnNo();
                
                //Getting Pass
                PunchCard Membership            = new();
                Membership.Type                 = Pass_Type_GModel.PunchCard; 
                Membership.Background_Color     = model.Background_Color;
                Membership.Label_Color          = model.Label_Color;
                Membership.Foreground_Color     = model.Foreground_Color;
                Membership.Localized_Name       = model.Logo_Text;
                Membership.Terms_And_Conditions = model.Terms_And_Conditions;
                Membership.Privacy_Policy       = model.Privacy_Policy;
                Membership.Logo_Url             = model.Logo_Url;
                Membership.Logo_Text            = model.Logo_Text;
                Membership.Organization_Name    = "ArenasPass";
                Membership.Serial_Number        = serialNo.ToString();
                Membership.Description          = string.IsNullOrEmpty(model.Description)?"N/A": model.Description;
                Membership.Recipient_Name       = model.Recipient_Name;
                Membership.Sender_Name          = model.Sender_Name;
                Membership.Message              = model.Message;  
                Membership.Code_Type            = model.Code_Type;
                Membership.Expiration_Date      = model.Expiration_Date;
                Membership.TenantId             = _contextAccessor.GetTenantId();
                Membership.Card_Holder_Title    = model.Card_Holder_Title;
                Membership.Card_holder_Name     = model.Card_holder_Name;
                Membership.Issuer               = _contextAccessor.GetCompanyName();
                Membership.Email                = model.Email;
                Membership.Phone                = model.Phone;
                Membership.Address              = model.Address;
                Membership.Pass_Status          = Pass_Redemption_Status_GModel.Template;

                Membership.Total_Punches        = model.Total_Punches;
                Membership.Current_Punches      = model.Current_Punches;
                Membership.Reward_Details       = model.Reward_Details;


                var res =await _iRepo.AddReturn(Membership);
                
                if (res == null)
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Error in inserting Punch card Please try again later";
                }

                await _iRepo.Save();
                //Saving the Gift Card
                return giftResponse;
            }
            catch (Exception ex)
            {
                await Rollback();
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
        public async Task<ResponseModel<string>> UpdateGiftCard(Apple_Passes_Punch_Card_Model GiftCard)
        {
           try
            {
                ResponseModel<string> giftResponse = new()
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

             
                PunchCard Membership           = await _iRepo.FindOne(x=>x.Id==GiftCard.Id);
                if (Membership == null) { giftResponse.Status_Code = "404"; giftResponse.Description = "No reocrd found"; return giftResponse; }

               
                Membership.Background_Color     = !string.IsNullOrEmpty(GiftCard.Background_Color) ? GiftCard.Background_Color : Membership.Background_Color;
                Membership.Label_Color          = !string.IsNullOrEmpty(GiftCard.Label_Color) ? GiftCard.Label_Color : Membership.Label_Color;
                Membership.Foreground_Color     = !string.IsNullOrEmpty(GiftCard.Foreground_Color) ? GiftCard.Foreground_Color : Membership.Foreground_Color;
                Membership.Localized_Name       = !string.IsNullOrEmpty(GiftCard.Logo_Text) ? GiftCard.Logo_Text : Membership.Localized_Name;
                Membership.Terms_And_Conditions = !string.IsNullOrEmpty(GiftCard.Terms_And_Conditions) ? GiftCard.Terms_And_Conditions : Membership.Terms_And_Conditions;
                Membership.Privacy_Policy       = !string.IsNullOrEmpty(GiftCard.Privacy_Policy) ? GiftCard.Privacy_Policy : Membership.Privacy_Policy;
                Membership.Logo_Url             = !string.IsNullOrEmpty(GiftCard.Logo_Url) ? GiftCard.Logo_Url : Membership.Logo_Url;
                Membership.Logo_Text            = !string.IsNullOrEmpty(GiftCard.Logo_Text) ? GiftCard.Logo_Text : Membership.Logo_Text;
                Membership.Organization_Name    = "ArenasPass"; 
                Membership.Description          = !string.IsNullOrEmpty(GiftCard.Description) ? GiftCard.Description : Membership.Description;
                Membership.Recipient_Name       = !string.IsNullOrEmpty(GiftCard.Recipient_Name) ? GiftCard.Recipient_Name : Membership.Recipient_Name;
                Membership.Sender_Name          = !string.IsNullOrEmpty(GiftCard.Sender_Name) ? GiftCard.Sender_Name : Membership.Sender_Name;
                Membership.Message              = !string.IsNullOrEmpty(GiftCard.Message) ? GiftCard.Message : Membership.Message;
                Membership.Code_Type            = !string.IsNullOrEmpty(GiftCard.Code_Type) ? GiftCard.Code_Type : Membership.Code_Type;
                Membership.Expiration_Date      = GiftCard.Expiration_Date ?? Membership.Expiration_Date;
                Membership.Card_Holder_Title    = !string.IsNullOrEmpty(GiftCard.Card_Holder_Title) ? GiftCard.Card_Holder_Title : Membership.Card_Holder_Title;
                Membership.Card_holder_Name     = !string.IsNullOrEmpty(GiftCard.Card_holder_Name) ? GiftCard.Card_holder_Name : Membership.Card_holder_Name;
                Membership.Issuer               = _contextAccessor.GetCompanyName();
                Membership.Email                = !string.IsNullOrEmpty(GiftCard.Email) ? GiftCard.Email : Membership.Email;
                Membership.Phone                = !string.IsNullOrEmpty(GiftCard.Phone) ? GiftCard.Phone : Membership.Phone;
                Membership.Address              = !string.IsNullOrEmpty(GiftCard.Address) ? GiftCard.Address : Membership.Address;
                
                Membership.Total_Punches        = GiftCard.Total_Punches==0? GiftCard.Total_Punches:Membership.Total_Punches;
                Membership.Current_Punches      = GiftCard.Current_Punches;
                Membership.Reward_Details       = !string.IsNullOrEmpty(GiftCard.Reward_Details)?GiftCard.Reward_Details : Membership.Reward_Details;
               
                
                _iRepo.Update(Membership);
                await _iRepo.Save();

                return giftResponse;
            }
            catch (Exception ex)
            {
                ResponseModel<string> giftResponse = new()
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
