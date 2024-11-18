using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Membership_Card_Service : Base_Service<MembershipCard>, IMembership_Card_Service
    {
        public Membership_Card_Service
        (
            IUnitOfWork unitOfWork, 
            IMembership_Card_Repo iRepo
        ) : base(unitOfWork, iRepo)
        {
        }
    }
}
