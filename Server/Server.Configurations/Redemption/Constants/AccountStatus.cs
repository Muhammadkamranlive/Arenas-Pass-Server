using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Configurations
{
    public static  class AccountStatus
    {
      
        public const string Open              = "Open";
        public const string Closed            = "Closed";
        public const string PartialEmpty      = "PartialEmpty";
        public const string PendingForSend    = "PendingForSend";
    }
}
