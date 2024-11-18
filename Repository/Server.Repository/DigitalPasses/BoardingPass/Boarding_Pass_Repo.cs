using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Boarding_Pass_Repo : Repo<BoardingPass>, IBoarding_Pass_Repo
    {
        public Boarding_Pass_Repo
        (
            ERPDb coursecontext, 
            IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {

        }
    }
}
