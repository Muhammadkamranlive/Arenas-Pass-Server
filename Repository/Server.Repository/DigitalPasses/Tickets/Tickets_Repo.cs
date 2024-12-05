using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Tickets_Repo : Repo<EventTicket>, ITickets_Repo
    {
        public Tickets_Repo
        (
            ERPDb coursecontext, 
            IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
