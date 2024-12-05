using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Server.Repository
{
    public class Gift_Cards_Repo : Repo<GiftCard>, IGift_Cards_Repo
    {
        private readonly ERPDb dpt;
        public Gift_Cards_Repo(ERPDb coursecontext, IHttpContextAccessor httpContextAccessor) : base(coursecontext, httpContextAccessor)
        {
            dpt = coursecontext;
        }

        
    }
}
