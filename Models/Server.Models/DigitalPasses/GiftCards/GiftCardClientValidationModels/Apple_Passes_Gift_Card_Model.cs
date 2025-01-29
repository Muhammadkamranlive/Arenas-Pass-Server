using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Apple_Passes_Gift_Card_Model:WalletPassModel
    {
        public decimal  Balance              { get; set; }
        public string   Currency_Code        { get; set; }
        public string?  Currency_Sign        { get; set; }
    }
}
