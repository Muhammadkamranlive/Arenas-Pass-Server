using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Repository;
using Server.BaseService;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.DigitalPasses.WalletPasses
{
    public class Wallet_Pass_Service : Base_Service<WalletPass>, IWallet_Pass_Service
    {
        private readonly ERPDb _dbt;
        public Wallet_Pass_Service
        (
            IUnitOfWork unitOfWork, 
            IWallet_Passes_Repo genericRepository,
            ERPDb eRPDb
         ) : base(unitOfWork, genericRepository)
        {
            _dbt = eRPDb;
        }


        public dynamic GetAllByParent()
        {
            try
            {
                var allWalletPasses = _dbt.Set<WalletPass>().ToList();

                var giftCards    = allWalletPasses.OfType<GiftCard>().ToList();
                var punchCards   = allWalletPasses.OfType<PunchCard>().ToList();
                var loyaltyCards = allWalletPasses.OfType<LoyaltyCard>().ToList();

                return new { GiftCards = giftCards, PunchCards = punchCards, LoyaltyCards = loyaltyCards };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
