using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using Server.Configurations;

namespace Server.Services
{
    public class Voucher_Service : Base_Service<Voucher>, IVoucher_Service
    {
        #region Constructor
        private readonly IApple_Passes_Service    _applePasses_Service;
        private readonly ITransaction_No_Service  tnService;
        private readonly IVoucher_Repo            _iRepo;
        private readonly IGet_Tenant_Id_Service   _contextAccessor;

        public Voucher_Service
        (
            IApple_Passes_Service    applePasses_Service,
            ITransaction_No_Service  tn,
            IUnit_Of_Work_Repo              unitOfWork,
            IVoucher_Repo            iRepo,
            IGet_Tenant_Id_Service   contextAccessor

        ) :base(unitOfWork,iRepo)
        {
            _applePasses_Service = applePasses_Service;
            tnService            = tn;
            _iRepo               = iRepo;
            _contextAccessor     = contextAccessor;
        }

        public async Task<ResponseModel<string>> DeleteCard(int GiftCardId, int tenantId)
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
        public async Task<ResponseModel<string>> GenerateCard(Apple_Passes_Voucher_Model model)
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
                int serialNo                 = await tnService.GetTxnNo();
                //Getting Pass
                Voucher voucher              = new();
                voucher.Type                 = Pass_Type_GModel.EventTicket; 
                voucher.Background_Color     = model.Background_Color;
                voucher.Label_Color          = model.Label_Color;
                voucher.Foreground_Color     = model.Foreground_Color;
                voucher.Localized_Name       = model.Logo_Text;
                voucher.Terms_And_Conditions = model.Terms_And_Conditions;
                voucher.Privacy_Policy       = model.Privacy_Policy;
                voucher.Logo_Url             = model.Logo_Url;
                voucher.Logo_Text            = model.Logo_Text;
                voucher.Organization_Name    = "ArenasPass";
                voucher.Serial_Number        = serialNo.ToString();
                voucher.Description          = string.IsNullOrEmpty(model.Description)?"N/A": model.Description;
                voucher.Recipient_Name       = model.Recipient_Name;
                voucher.Sender_Name          = model.Sender_Name;
                voucher.Message              = model.Message;  
                voucher.Code_Type            = model.Code_Type;
                voucher.Expiration_Date      = model.Expiration_Date;
                voucher.TenantId             = _contextAccessor.GetTenantId();
                voucher.Card_Holder_Title    = model.Card_Holder_Title;
                voucher.Card_holder_Name     = model.Card_holder_Name;
                voucher.Issuer               = _contextAccessor.GetCompanyName();
                voucher.Email                = model.Email;
                voucher.Phone                = model.Phone;
                voucher.Address              = model.Address;
                voucher.Pass_Status          = Pass_Redemption_Status_GModel.Template;
   
                voucher.Discount_Percentage  = model.Discount_Percentage;
                var res =await _iRepo.AddReturn(voucher);
                
                if (res == null)
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Error in inserting Gift Card Please try again later";
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
        /// <param name="Model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseModel<string>> UpdateCard(Apple_Passes_Voucher_Model Model)
        {
           try
            {
                ResponseModel<string> giftResponse = new()
                {
                    Status_Code              = "200",
                    Description              = "OK",
                    Response                 = null
                };


                Voucher voucher              = await _iRepo.FindOne(x=>x.Id==Model.Id);
                if (voucher == null) { giftResponse.Status_Code = "404"; giftResponse.Description = "No reocrd found"; return giftResponse; }

               
                voucher.Background_Color     = !string.IsNullOrEmpty(Model.Background_Color) ? Model.Background_Color : voucher.Background_Color;
                voucher.Label_Color          = !string.IsNullOrEmpty(Model.Label_Color) ? Model.Label_Color : voucher.Label_Color;
                voucher.Foreground_Color     = !string.IsNullOrEmpty(Model.Foreground_Color) ? Model.Foreground_Color : voucher.Foreground_Color;
                voucher.Localized_Name       = !string.IsNullOrEmpty(Model.Logo_Text) ? Model.Logo_Text : voucher.Localized_Name;
                voucher.Terms_And_Conditions = !string.IsNullOrEmpty(Model.Terms_And_Conditions) ? Model.Terms_And_Conditions : voucher.Terms_And_Conditions;
                voucher.Privacy_Policy       = !string.IsNullOrEmpty(Model.Privacy_Policy) ? Model.Privacy_Policy : voucher.Privacy_Policy;
                voucher.Logo_Url             = !string.IsNullOrEmpty(Model.Logo_Url) ? Model.Logo_Url : voucher.Logo_Url;
                voucher.Logo_Text            = !string.IsNullOrEmpty(Model.Logo_Text) ? Model.Logo_Text : voucher.Logo_Text;
                voucher.Organization_Name    = "ArenasPass"; 
                voucher.Description          = !string.IsNullOrEmpty(Model.Description) ? Model.Description : voucher.Description;
                voucher.Recipient_Name       = !string.IsNullOrEmpty(Model.Recipient_Name) ? Model.Recipient_Name : voucher.Recipient_Name;
                voucher.Sender_Name          = !string.IsNullOrEmpty(Model.Sender_Name) ? Model.Sender_Name : voucher.Sender_Name;
                voucher.Message              = !string.IsNullOrEmpty(Model.Message) ? Model.Message : voucher.Message;
                voucher.Code_Type            = !string.IsNullOrEmpty(Model.Code_Type) ? Model.Code_Type : voucher.Code_Type;
                voucher.Expiration_Date      = Model.Expiration_Date ?? voucher.Expiration_Date;
                voucher.Card_Holder_Title    = !string.IsNullOrEmpty(Model.Card_Holder_Title) ? Model.Card_Holder_Title : voucher.Card_Holder_Title;
                voucher.Card_holder_Name     = !string.IsNullOrEmpty(Model.Card_holder_Name) ? Model.Card_holder_Name : voucher.Card_holder_Name;
                voucher.Issuer               = _contextAccessor.GetCompanyName();
                voucher.Email                = !string.IsNullOrEmpty(Model.Email) ? Model.Email : voucher.Email;
                voucher.Phone                = !string.IsNullOrEmpty(Model.Phone) ? Model.Phone : voucher.Phone;
                voucher.Address              = !string.IsNullOrEmpty(Model.Address) ? Model.Address : voucher.Address;
                voucher.Discount_Percentage  = !string.IsNullOrEmpty(Model.Description)? Model.Discount_Percentage:voucher.Discount_Percentage;
               
                _iRepo.Update(voucher);
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
