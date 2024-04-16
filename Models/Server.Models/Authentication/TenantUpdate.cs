using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class TenantUpdate
    {
        public string PhoneNumber       { get; set; }
        public string AddressLine1      { get; set; }
        public string AddressLine2      { get; set; }
        public string City              { get; set; }
        public string State             { get; set; }
        public string Country           { get; set; }
        public string ZipCode           { get; set; }
        public decimal Longitude        { get; set; }
        public decimal Latitude         { get; set; }
        public string isMailingAddress  { get; set; }
        public string isPhysicalAddress { get; set; }
        public string whosisCompany     { get; set; }
        public int noDomesticEmployee   { get; set; }
        public int noInterEmployee      { get; set; }
        public int noDomesticContractor { get; set; }
        public int noInterContractor    { get; set; }
        public string EIN               { get; set; }
        public string BussinessType     { get; set; }
        public string LegalName         { get; set; }
        public string FillingFormIRS    { get; set; }
        public string Industry          { get; set; }
    }
}
