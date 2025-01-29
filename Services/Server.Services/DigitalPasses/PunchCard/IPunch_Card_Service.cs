using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.DigitalPasses
{
    public interface IPunch_Card_Service :IBase_Service<PunchCard>
    {
        Task<ResponseModel<string>> DeleteGiftCard(int GiftCardId, int tenantId);
        Task<ResponseModel<string>> GenerateCard(Apple_Passes_Punch_Card_Model model);
        Task<ResponseModel<string>> UpdateGiftCard(Apple_Passes_Punch_Card_Model GiftCard);
    }
}
