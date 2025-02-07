using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Contact_Page_Repo:Repo<ContactPage>,IContact_Page_Repo
    {
        public Contact_Page_Repo(ERPDb dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {

        }
    }
}
