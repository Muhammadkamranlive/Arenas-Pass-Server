using System;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Repository
{
    public class Wallet_Pass_Repo : Repo<WalletPass>,IWallet_Pass_Repo
    {
        public Wallet_Pass_Repo(ERPDb coursecontext, IHttpContextAccessor httpContextAccessor) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
