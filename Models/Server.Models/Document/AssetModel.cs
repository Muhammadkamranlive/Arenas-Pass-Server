namespace Server.Models
{
    public class AssetModel
    {
        public Guid Id                  { get; set; }
        public string userId            { get; set; }
        public string ItemName          { get; set; }
        public string Category          { get; set; }
        public string Manufacturer      { get; set; }
        public decimal Price            { get; set; }
        public string ItemCode          { get; set; }
        public string ModelNo           { get; set; }
        public string SerialOrLicenseNo { get; set; }
        public DateTime? ExpiryDate     { get; set; }
        public DateTime? WarrantyTill   { get; set; }
        public string Note              { get; set; }
    }
}
