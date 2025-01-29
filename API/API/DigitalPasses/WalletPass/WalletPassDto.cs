using Server.Configurations;

namespace API.API.DigitalPasses.WalletPass
{
    public class WalletPassDto
    {
        public int Id                        { get; set; }
        public string Type                   { get; set; }
        public string Background_Color       { get; set; }
        public string Foreground_Color       { get; set; }
        public string Label_Color            { get; set; }
        public string Serial_Number          { get; set; }
        public int TenantId                  { get; set; }
        public string? Logo_Url              { get; set; }
        public string? Logo_Text             { get; set; }
        public string? Card_holder_Name      { get; set; }
        public string? Card_Holder_Title     { get; set; }
        public string? Code_Type             { get; set; }
        public DateTime? Expiration_Date     { get; set; }
        public DateTime? Relevant_Date       { get; set; }
        public DateTime? Effective_Date      { get; set; }
        public string? Recipient_Name        { get; set; }
        public string? Sender_Name           { get; set; }
        public string? Email                 { get; set; }
        public string? Phone                 { get; set; }
        public string? Address               { get; set; }
        public string? Webiste               { get; set; }
        public string? Web_Service_URL       { get; set; }
        public string? Privacy_Policy        { get; set; }
        public string? Terms_And_Conditions  { get; set; }                           
        public string? Message               { get; set; }
        public string? Description           { get; set; }
        public string Pass_Status            { get; set; }
         //Gift Card Enteries
         public string? Currency_Code         { get; set; }
         public decimal? Balance              { get; set; }
         public string? Currency_Sign         { get; set; }

         //Voucher Enteries 
         public decimal? Amount               { get; set; }
        //Copoun
        public string? Offer_Code             { get; set; }
        public decimal? Discount_Percentage   { get; set; }
        public string? Is_Redeemed            { get; set; }
        //EventTicket
        public string? EventName               { get; set; }
        public string? VenueName               { get; set; }
        public string? SeatInfo                { get; set; }
        public string? TicketNumber            { get; set; }
        public string? EntryGate               { get; set; }
        public string? Ticket_Type             { get; set; }
        //LoyaltyCard                         
        public string? Program_Name            { get; set; }
        public int? Points_Balance             { get; set; }
        public string? Reward_Details          { get; set; }
        //Membership card                     
        public string? Member_Name             { get; set; }
        public string? Membership_Number       { get; set; }
        public string? Additional_Benefits     { get; set; }
        public string? Status                  { get; set; }
        //Punch Cards
        public string? Punch_Title         { get; set; }
        public int? Total_Punches          { get; set; }
        public int? Current_Punches        { get; set; }
    }
}