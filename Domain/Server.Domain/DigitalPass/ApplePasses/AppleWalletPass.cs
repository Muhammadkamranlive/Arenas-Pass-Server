using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class AppleWalletPass : WalletPass
    {
        public string PassTypeIdentifier { get; set; }
        public string TeamIdentifier     { get; set; }
        public string PassStyle          { get; set; }  
        public string NFCFields          { get; set; }  
    }

}
