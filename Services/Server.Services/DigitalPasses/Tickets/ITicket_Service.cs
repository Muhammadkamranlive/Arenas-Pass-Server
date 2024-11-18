using Server.Core;
using Server.Domain;
using Server.Models;

namespace Server.Services
{
    public interface ITicket_Service:IBase_Service<Ticket>
    {
        Task<ResponseModel<string>> GenerateTicket(Apple_Passes_Ticket_Model TicketModel);
    }
}
