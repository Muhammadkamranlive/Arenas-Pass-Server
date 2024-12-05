using Server.Models;

namespace Server.Services
{
    public interface IApple_Passes_Service
    {
        Task<ResponseModel<string>> GiftCards(Apple_Passes_Gift_Card_Model model, string serialNo);
        Task<ResponseModel<string>> Coupons(Apple_Passes_Coupon_Model model, string SerialNo);
        Task<ResponseModel<string>> BoardingPass(Boarding_Pass_Model model, string SerialNo);
        Task<ResponseModel<string>> Vouchers(Apple_Passes_Voucher_Model model, string SerialNo);
        Task<ResponseModel<string>> GenerateLoyaltyCard(Apple_Passes_Loyalty_Card_Model model, string SerialNo);
        Task<ResponseModel<string>> GenerateTicket(Apple_Passes_Ticket_Model model, string SerialNo);
        Task<ResponseModel<string>> GenerateMembershipCard(Apple_Passes_Membership_Card_Model model, string SerialNo);
        Task<ResponseModel<string>> PunchCards(Apple_Passes_Punch_Card_Model model, string SerialNo);

    }
}
