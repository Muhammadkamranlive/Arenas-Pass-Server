namespace Server.Models;

public class ArenasPaymentSection
{
    
}

public class PaymentRequest
{
    public long Amount { get; set; }
    public string Email { get; set; }
}

public class ConfirmPaymentRequest
{
    public string PaymentIntentId { get; set; }
    public string Email { get; set; }
}