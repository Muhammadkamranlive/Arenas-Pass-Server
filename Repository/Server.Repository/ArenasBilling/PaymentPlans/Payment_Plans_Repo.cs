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
    public class Payment_Plans_Repo : Repo<Payment_Plans>, IPayment_Plans_Repo
    {
        public Payment_Plans_Repo(ERPDb coursecontext, IHttpContextAccessor httpContextAccessor) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
