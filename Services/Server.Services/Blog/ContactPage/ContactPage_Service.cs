using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class ContactPage_Service:Base_Service<ContactPage>,IContactPage_Service
    {
        public ContactPage_Service(IUnitOfWork unitOfWork, IContactPage_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
