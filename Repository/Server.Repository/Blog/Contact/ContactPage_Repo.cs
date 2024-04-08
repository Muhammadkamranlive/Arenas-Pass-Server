using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class ContactPage_Repo:Repo<ContactPage>,IContactPage_Repo
    {
        public ContactPage_Repo(ERPDb dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {

        }
    }
}
