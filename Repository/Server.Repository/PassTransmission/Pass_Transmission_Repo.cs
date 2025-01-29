using System;
using Server.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Server.Domain.PassTransmission;

namespace Server.Repository
{
    public class Pass_Transmission_Repo : Repo<Pass_Transmission>, IPass_Transmission_Repo
    {
        public Pass_Transmission_Repo
        (
            ERPDb coursecontext, 
            IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
