﻿using Server.Core;
using Server.Domain;
using Server.Models;

namespace Server.Services
{
    public interface IGift_Card_Service:IBase_Service<GiftCard>
    {
        Task<ResponseModel<string>> GenerateGiftCard(Apple_Passes_Gift_Card_Model GiftCard);
        Task<ResponseModel<string>> UpdateGiftCard(Apple_Passes_Gift_Card_Model GiftCard);
        Task<ResponseModel<string>> PublishGiftCardToCommunity(IList<int> listofgiftcards);
        Task<ResponseModel<string>> DeleteGiftCard(int GiftCardId, int tenantId);
        ResponseModel<string> ValidateGiftCardForRedemption(GiftCard GiftCard, Redeem_Gift_Card_Model model);
    }
}
