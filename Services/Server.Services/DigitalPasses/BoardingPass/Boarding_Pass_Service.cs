using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Boarding_Pass_Service : Base_Service<BoardingPass>, IBoarding_Pass_Service
    {
        public Boarding_Pass_Service
        (
            IUnitOfWork unitOfWork, 
            IBoarding_Pass_Repo iRepo
        ) : base(unitOfWork, iRepo)
        {
        }
    }
}
