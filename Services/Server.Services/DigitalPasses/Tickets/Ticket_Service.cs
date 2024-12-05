using Server.UOW;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Ticket_Service : Base_Service<EventTicket>, ITicket_Service
    {
        #region Constructor
        private readonly IApple_Passes_Service _applePasses_Service;
        private readonly ITickets_Repo         _ticketets_Repo;
        public Ticket_Service
        (
            IUnitOfWork           unitOfWork, 
            IApple_Passes_Service appService,
            ITickets_Repo iRepo
        ) : base(unitOfWork, iRepo)
        {
            _applePasses_Service = appService;
            _ticketets_Repo      = iRepo;
        }
        #endregion




        /// <summary>
        /// Generate Tickets
        /// </summary>
        /// <param name="Ticket"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateTicket(Apple_Passes_Ticket_Model TicketModel)
        {
            try
            {
                ResponseModel<string> ticketResponse = new()
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                //// Getting Pass
                //ticketResponse  =  _applePasses_Service.GenerateTicket(TicketModel);
                //if (ticketResponse.Status_Code != "200") { return ticketResponse; }

                //Ticket ticket               = new();
                //ticket.Type                 = "Ticket";
                //ticket.Apple_Pass           = (byte[])ticketResponse.Response;
                //ticket.Background_Color     = TicketModel.Background_Color;
                //ticket.Label_Color          = TicketModel.Label_Color;
                //ticket.Foreground_Color     = TicketModel.Foreground_Color;
                //ticket.Localized_Name       = TicketModel.Logo_Text;
                //ticket.Terms_And_Conditions = TicketModel.Privacy_Policy;
                //ticket.Logo                 = TicketModel.Logo_Url;
                //ticket.Organization_Name    = TicketModel.Organization_Name;
                //ticket.Serial_Number        = TicketModel.Serial_Number;
                //ticket.Description          = TicketModel.Description;
                //ticket.Web_Service_URL      = "N/A";
                //ticket.Authentication_Token = "";

                //// Set properties specific to Ticket
                //ticket.Event_Name           = TicketModel.Event_Name;
                //ticket.Venue                = TicketModel.Venue;
                //ticket.Seat_Number          = TicketModel.Seat_Number;
                //ticket.Event_Date           = TicketModel.Event_Date;
                //ticket.Barcode_Type         = "N/A";
                //ticket.Barcode_Format       = "N/A";

                //await _ticketets_Repo.Transaction();
                //await _ticketets_Repo.Add(ticket);
                //await _ticketets_Repo.Commit();

                return ticketResponse;
            }
            catch (Exception ex)
            {
                await _ticketets_Repo.Rollback();
                return new ResponseModel<string>
                {
                    Status_Code = "500",
                    Description = ex.Message,
                    Response = "Error Occurred"
                };
            }
        }


    }
}
