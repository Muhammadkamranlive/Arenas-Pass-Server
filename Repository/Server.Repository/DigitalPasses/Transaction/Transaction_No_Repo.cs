using System;
using System.Linq;
using System.Text;
using Server.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Server.Domain.DigitalPass.Transaction;

namespace Server.Repository
{
    public class Transaction_No_Repo : Repo<Transaction_No>, ITransaction_No_Repo
    {
        public Transaction_No_Repo
        (
            ERPDb coursecontext, IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {

        }
    }
}
