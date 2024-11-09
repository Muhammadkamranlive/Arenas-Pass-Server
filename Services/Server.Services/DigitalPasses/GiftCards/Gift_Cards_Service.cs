using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.BaseService;

namespace Server.Services
{
    public class Gift_Cards_Service : Base_Service<GiftCard>, IGift_Cards_Service
    {
        public Gift_Cards_Service
        (
            IUnitOfWork unitOfWork, 
            IRepo<GiftCard> genericRepository
        ) : base(unitOfWork, genericRepository)
        {

        }


    }
}
