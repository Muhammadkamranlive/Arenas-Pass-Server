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
        public bool isMailingAddress    { get; set; }
        public bool isPhysicalAddress   { get; set; }
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
