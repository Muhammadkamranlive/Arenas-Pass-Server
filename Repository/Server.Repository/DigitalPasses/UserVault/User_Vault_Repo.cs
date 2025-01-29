using System;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Repository
{
    public class User_Vault_Repo : Repo<UsersVault>, IUser_Vault_Repo
    {
        public User_Vault_Repo(ERPDb coursecontext, IHttpContextAccessor httpContextAccessor) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
