namespace Server.Domain;

public class UserBillingDetail
{
    public int    Id          { get; set; }
    public string Name        { get; set; }
    public string PhoneNumber { get; set; }
    public string Email       { get; set; }
    public string Address     { get; set; }
    public string City        { get; set; }
    public string State       { get; set; }
    public string Zip         { get; set; }
    public string Country     { get; set; }
    
}