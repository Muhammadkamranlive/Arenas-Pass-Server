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
    public class Feature_List_Repo : Repo<PaymentFeatureList>, IFeature_List_Repo
    {
        public Feature_List_Repo(ERPDb coursecontext, IHttpContextAccessor httpContextAccessor) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
