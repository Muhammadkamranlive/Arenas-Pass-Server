using Azure;
using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Server.Domain.PassTransmission;

namespace Server.Services
{
    public interface ISend_Pass_Customer_Service:IBase_Service<Pass_Transmission>
    {
        Task<ResponseModel<string>> SendtoUser(IList<string> listCards);
        Task<IList<WalletPass>> GetWalletPasses(); 
    }
}
