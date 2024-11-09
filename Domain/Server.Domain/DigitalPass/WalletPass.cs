using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class WalletPass
    {
        public int Id                               { get; set; }
        public string Type                          { get; set; }  
        public string Logo                          { get; set; }
        public string BackgroundColor               { get; set; }
        public string ForegroundColor               { get; set; }
        public string LabelColor                    { get; set; }
        public string OrganizationName              { get; set; }
        public string SerialNumber                  { get; set; }
        public DateTime? ExpirationDate             { get; set; }
        public string Barcode                       { get; set; }
        public string BarcodeFormat                 { get; set; }  
        public DateTime? RelevantDate               { get; set; }
        public string LocalizedName                 { get; set; }
        public string TermsAndConditions            { get; set; }
        public string Description                   { get; set; }
        public List<Location> Locations             { get; set; }  
        public string WebServiceURL                 { get; set; }
        public string AuthenticationToken           { get; set; }
        public List<int> AssociatedStoreIdentifiers { get; set; }
        public decimal? Balance                     { get; set; }
        public int? RewardsPoints                   { get; set; }
        public string CouponCode                    { get; set; }
        public decimal? DiscountAmount              { get; set; }
        public List<Offer> Offers                   { get; set; }
    }


    public class Location
    {
        public int Id                             { get; set; }
        public double Latitude                    { get; set; }
        public double Longitude                   { get; set; }
        public string VenueDetails                { get; set; }
    }

    public class Offer
    {
        public int Id                            { get; set; }
        public string Title                      { get; set; }
        public string Description                { get; set; }
    }

}
