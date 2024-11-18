using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Apple_Pass_Account
    {
        public int    Id                   { get; set; }
        public string Pass_Type_Identifier { get; set; }
        public string Team_Identifier      { get; set; }
        public string Serial_Number        { get; set; }
        public int    TenantId             { get; set; }
        public DateTime CreatedAt          { get; set; }
        public Apple_Pass_Account()
        {
            CreatedAt= DateTime.Now;
        }
    }
    
}
