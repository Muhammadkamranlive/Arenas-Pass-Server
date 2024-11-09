using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models.Payments
{
    public class EmployeePayment
    {
        public string EmployeeEmail          { get; set; }
        public string EmployeeName           { get; set; }
        public string EmployeeAddress        { get; set; }
        public string EmployeeZipcode        { get; set; }
        public string EmployeeCity           { get; set; }
        public string EmployeeCountry        { get; set; }="USA";
        public string PriceId                { get; set; }
        public string CompanyEmail           { get; set; }
        public string CompanyAddress         { get; set; }
        public string CompanyLogo            { get; set; }
        public string CompanyPhone           { get; set; }
    }
}
