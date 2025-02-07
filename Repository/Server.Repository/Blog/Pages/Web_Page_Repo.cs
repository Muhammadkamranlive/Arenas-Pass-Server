using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Web_Page_Repo:Repo<WebPages>, IWeb_Page_Repo
    {
        public Web_Page_Repo(ERPDb dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {

        }
    }
}
