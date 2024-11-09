using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class GoogleWalletPass : WalletPass
    {
        public string IssuerId         { get; set; }
        public string ClassReferenceId { get; set; }
        public string UserToken        { get; set; }
    }

}
