using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IUserBilling_Address_Service:IBase_Service<UserBillingDetail>
    {
        Task<ResponseModel<string>> AddOrUpdateBilingDetail(UserBillingDetail model);
    }
}
