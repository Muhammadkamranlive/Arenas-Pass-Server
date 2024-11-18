using Server.Models;

namespace Server.Services
{
    public interface IApple_Passes_Service
    {
        Task<ResponseModel<string>> GiftCards(Apple_Passes_Gift_Card_Model GiftCard);
        ResponseModel<string> Coupons(Apple_Passes_Coupon_Model coupon);
        ResponseModel<string> Vouchers(Apple_Passes_Voucher_Model voucher);
        ResponseModel<string> GenerateTicket(Apple_Passes_Ticket_Model ticket);
        Task<ResponseModel<string>> GenerateLoyaltyCard(Apple_Passes_Loyalty_Card_Model loyaltyCard);
        ResponseModel<string> GenerateMembershipCard(Apple_Passes_Membership_Card_Model membershipCard);
        Task<ResponseModel<string>> PunchCards(Apple_Passes_Punch_Card_Model punchCard);
    }
}
