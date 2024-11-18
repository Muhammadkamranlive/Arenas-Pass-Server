using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Loyalty_Card_Service : Base_Service<LoyaltyCard>, ILoyalty_Card_Service
    {
        public Loyalty_Card_Service
        (
            IUnitOfWork unitOfWork, 
            ILoyalty_Card_Repo iRepo
        ) : base(unitOfWork, iRepo)
        {
        }
    }
}
