using Server.Models;

namespace Server.Services
{
    public interface IApple_Passes_Service
    {
        Task<ResponseModel<string>> GiftCards(Apple_Passes_Gift_Card_Model GiftCard);
    }
}
