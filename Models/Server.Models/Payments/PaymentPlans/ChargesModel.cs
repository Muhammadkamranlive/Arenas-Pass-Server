namespace Server.Models;

public class ChargesModel
{
    public int        Id                { get; set; }
    public string     ChargeName        { get; set; }
    public string     ChargeType        { get; set; }  
    public decimal?   ChargeAmount      { get; set; }  
    public string     ChargeDescription { get; set; } 
    public string     Status            { get; set; }
}