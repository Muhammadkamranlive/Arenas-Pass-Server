using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Membership_Card_Repo : Repo<MembershipCard>,IMembership_Card_Repo
    {
        public Membership_Card_Repo
        (
            ERPDb coursecontext, 
            IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
