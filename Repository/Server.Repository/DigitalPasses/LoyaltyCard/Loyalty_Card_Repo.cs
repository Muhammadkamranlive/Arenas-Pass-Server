using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Loyalty_Card_Repo : Repo<LoyaltyCard>, ILoyalty_Card_Repo
    {
        public Loyalty_Card_Repo
        (
            ERPDb coursecontext, 
            IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext,  httpContextAccessor)
        {
        }
    }
}
