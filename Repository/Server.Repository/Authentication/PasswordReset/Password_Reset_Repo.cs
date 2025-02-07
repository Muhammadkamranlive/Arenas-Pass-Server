using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Password_Reset_Repo : Repo<PasswordResetDomain>, IPassword_Reset_Repo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Password_Reset_Repo(ERPDb db, IHttpContextAccessor httpContextAccessor) : base(db,httpContextAccessor)
        {

        }
    }
}