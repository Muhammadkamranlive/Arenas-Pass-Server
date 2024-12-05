using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.DigitalPasses.WalletPasses
{
    public class Wallet_Pass_Service : Base_Service<WalletPass>, IWallet_Pass_Service
    {
        public Wallet_Pass_Service(IUnitOfWork unitOfWork, IWallet_Passes_Repo genericRepository) : base(unitOfWork, genericRepository)
        {
        }
    }
}
