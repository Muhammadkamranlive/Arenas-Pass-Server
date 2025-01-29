using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IWallet_Pass_Service:IBase_Service<WalletPass>
    {
        dynamic GetAllByParent();
    }
}
