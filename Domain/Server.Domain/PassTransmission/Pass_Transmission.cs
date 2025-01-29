using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain.PassTransmission
{
    public class Pass_Transmission
    {
        public int Id                   { get; set; }
        public int Card_Id              { get; set; }
        public string Email             { get; set; }
        public string Pass_Trans_Status { get; set; }
    }
}
