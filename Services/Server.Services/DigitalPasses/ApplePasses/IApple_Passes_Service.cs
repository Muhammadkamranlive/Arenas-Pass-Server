using Server.Models;

namespace Server.Services
{
    public interface IApple_Passes_Service
    {
        Task<ResponseModel<string>> GiftCards(Gift_Card_Pass_Model GiftCard);
    }
}
