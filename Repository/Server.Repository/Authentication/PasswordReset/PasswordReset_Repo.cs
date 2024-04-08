using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class PasswordReset_Repo : Repo<PasswordResetDomain>, IPasswordReset_Repo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PasswordReset_Repo(ERPDb db, IHttpContextAccessor httpContextAccessor) : base(db,httpContextAccessor)
        {

        }
    }
}