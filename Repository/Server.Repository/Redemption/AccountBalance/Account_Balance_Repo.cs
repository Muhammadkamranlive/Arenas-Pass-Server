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
    public class Account_Balance_Repo : Repo<Account_Balance>, IAccount_Balance_Repo
    {
        public Account_Balance_Repo
        (
            ERPDb coursecontext, IHttpContextAccessor httpContextAccessor) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
