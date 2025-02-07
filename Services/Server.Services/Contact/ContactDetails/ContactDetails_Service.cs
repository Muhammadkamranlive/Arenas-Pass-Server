using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class ContactDetails_Service:Base_Service<CONTACTDETAILS>, IContactDetails_Service
    {
        public ContactDetails_Service(IUnit_Of_Work_Repo unitOfWork, IContactDetails_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
