using Server.UOW;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;
using Server.Configurations;

namespace Server.Services
{
    public class Ticket_Service : Base_Service<EventTicket>, ITicket_Service
    {
        #region Constructor
        private readonly IApple_Passes_Service    _applePasses_Service;
        private readonly ITransaction_No_Service  tnService;
        private readonly ITickets_Repo            _iRepo;
        private readonly IGet_Tenant_Id_Service   _contextAccessor;

        public Ticket_Service
        (
            IApple_Passes_Service    applePasses_Service,
            ITransaction_No_Service  tn,
            IUnitOfWork              unitOfWork,
            ITickets_Repo            iRepo,
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
        public async Task<ResponseModel<string>> GenerateCard(Apple_Passes_Ticket_Model model)
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
                EventTicket Ticket          = new();
                Ticket.Type                 = Pass_Type_GModel.EventTicket; 
                Ticket.Background_Color     = model.Background_Color;
                Ticket.Label_Color          = model.Label_Color;
                Ticket.Foreground_Color     = model.Foreground_Color;
                Ticket.Localized_Name       = model.Logo_Text;
                Ticket.Terms_And_Conditions = model.Terms_And_Conditions;
                Ticket.Privacy_Policy       = model.Privacy_Policy;
                Ticket.Logo_Url             = model.Logo_Url;
                Ticket.Logo_Text            = model.Logo_Text;
                Ticket.Organization_Name    = "ArenasPass";
                Ticket.Serial_Number        = serialNo.ToString();
                Ticket.Description          = string.IsNullOrEmpty(model.Description)?"N/A": model.Description;
                Ticket.Recipient_Name       = model.Recipient_Name;
                Ticket.Sender_Name          = model.Sender_Name;
                Ticket.Message              = model.Message;  
                Ticket.Code_Type            = model.Code_Type;
                Ticket.Expiration_Date      = model.Expiration_Date;
                Ticket.TenantId             = _contextAccessor.GetTenantId();
                Ticket.Card_Holder_Title    = model.Card_Holder_Title;
                Ticket.Card_holder_Name     = model.Card_holder_Name;
                Ticket.Issuer               = _contextAccessor.GetCompanyName();
                Ticket.Email                = model.Email;
                Ticket.Phone                = model.Phone;
                Ticket.Address              = model.Address;
                Ticket.Pass_Status          = Pass_Redemption_Status_GModel.Template;
                
                Ticket.EventName            = model.EventName;
                Ticket.VenueName            = model.VenueName;
                Ticket.SeatInfo             = model.SeatInfo;   
                Ticket.TicketNumber         = model.TicketNumber;
                Ticket.EntryGate            = model.EntryGate;
                Ticket.Ticket_Type          = model.Ticket_Type;
                var res =await _iRepo.AddReturn(Ticket);
                
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
        public async Task<ResponseModel<string>> UpdateCard(Apple_Passes_Ticket_Model Model)
        {
           try
            {
                ResponseModel<string> giftResponse = new()
                {
                    Status_Code                 = "200",
                    Description                 = "OK",
                    Response                    = null
                };


                EventTicket Ticket          = await _iRepo.FindOne(x=>x.Id==Model.Id);
                if (Ticket == null) { giftResponse.Status_Code = "404"; giftResponse.Description = "No reocrd found"; return giftResponse; }

               
                Ticket.Background_Color     = !string.IsNullOrEmpty(Model.Background_Color) ? Model.Background_Color : Ticket.Background_Color;
                Ticket.Label_Color          = !string.IsNullOrEmpty(Model.Label_Color) ? Model.Label_Color : Ticket.Label_Color;
                Ticket.Foreground_Color     = !string.IsNullOrEmpty(Model.Foreground_Color) ? Model.Foreground_Color : Ticket.Foreground_Color;
                Ticket.Localized_Name       = !string.IsNullOrEmpty(Model.Logo_Text) ? Model.Logo_Text : Ticket.Localized_Name;
                Ticket.Terms_And_Conditions = !string.IsNullOrEmpty(Model.Terms_And_Conditions) ? Model.Terms_And_Conditions : Ticket.Terms_And_Conditions;
                Ticket.Privacy_Policy       = !string.IsNullOrEmpty(Model.Privacy_Policy) ? Model.Privacy_Policy : Ticket.Privacy_Policy;
                Ticket.Logo_Url             = !string.IsNullOrEmpty(Model.Logo_Url) ? Model.Logo_Url : Ticket.Logo_Url;
                Ticket.Logo_Text            = !string.IsNullOrEmpty(Model.Logo_Text) ? Model.Logo_Text : Ticket.Logo_Text;
                Ticket.Organization_Name    = "ArenasPass"; 
                Ticket.Description          = !string.IsNullOrEmpty(Model.Description) ? Model.Description : Ticket.Description;
                Ticket.Recipient_Name       = !string.IsNullOrEmpty(Model.Recipient_Name) ? Model.Recipient_Name : Ticket.Recipient_Name;
                Ticket.Sender_Name          = !string.IsNullOrEmpty(Model.Sender_Name) ? Model.Sender_Name : Ticket.Sender_Name;
                Ticket.Message              = !string.IsNullOrEmpty(Model.Message) ? Model.Message : Ticket.Message;
                Ticket.Code_Type            = !string.IsNullOrEmpty(Model.Code_Type) ? Model.Code_Type : Ticket.Code_Type;
                Ticket.Expiration_Date      = Model.Expiration_Date ?? Ticket.Expiration_Date;
                Ticket.Card_Holder_Title    = !string.IsNullOrEmpty(Model.Card_Holder_Title) ? Model.Card_Holder_Title : Ticket.Card_Holder_Title;
                Ticket.Card_holder_Name     = !string.IsNullOrEmpty(Model.Card_holder_Name) ? Model.Card_holder_Name : Ticket.Card_holder_Name;
                Ticket.Issuer               = _contextAccessor.GetCompanyName();
                Ticket.Email                = !string.IsNullOrEmpty(Model.Email) ? Model.Email : Ticket.Email;
                Ticket.Phone                = !string.IsNullOrEmpty(Model.Phone) ? Model.Phone : Ticket.Phone;
                Ticket.Address              = !string.IsNullOrEmpty(Model.Address) ? Model.Address : Ticket.Address;
                
                Ticket.EventName            = !string.IsNullOrEmpty(Model.EventName)? Model.EventName:Ticket.EventName;
                Ticket.VenueName            = !string.IsNullOrEmpty(Model.VenueName) ? Model.VenueName : Ticket.VenueName;
                Ticket.SeatInfo             = !string.IsNullOrEmpty(Model.SeatInfo)?Model.SeatInfo : Ticket.SeatInfo;
                Ticket.TicketNumber         = !string.IsNullOrEmpty(Model.TicketNumber)?Model.TicketNumber : Ticket.TicketNumber;
                Ticket.EntryGate            = !string.IsNullOrEmpty(Model.EntryGate) ? Model.EntryGate : Ticket.EntryGate;
                Ticket.Ticket_Type          = !string.IsNullOrEmpty(Model.Ticket_Type) ? Model.Ticket_Type : Ticket.Ticket_Type;

                _iRepo.Update(Ticket);
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
