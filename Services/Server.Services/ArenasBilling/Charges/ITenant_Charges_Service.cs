using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Server.Models;

namespace Server.Services
{
    public interface ITenant_Charges_Service:IBase_Service<TenantCharges>
    {
        Task<ResponseModel<string>> AddCharges(ChargesModel model);
        Task<ResponseModel<string>> UpdateCharges(ChargesModel model);
    }
}
