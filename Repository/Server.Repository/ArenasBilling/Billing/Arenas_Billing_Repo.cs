using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Repository
{
    public class Arenas_Billing_Repo : Repo<ArenasBilling>, IArenas_Billing_Repo
    {
        public Arenas_Billing_Repo
        (
            ERPDb coursecontext, IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
