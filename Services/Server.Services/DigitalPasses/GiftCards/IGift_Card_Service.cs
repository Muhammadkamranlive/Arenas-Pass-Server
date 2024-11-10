using Server.Core;
using Server.Domain;
using Server.Models;

namespace Server.Services
{
    public interface IGift_Card_Service
    {
        Task<ResponseModel<string>> GenerateGiftCard(Apple_Passes_Gift_Card_Model GiftCard);
    }
}
