using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Contact_Page_Service:Base_Service<ContactPage>,IContact_Page_Service
    {
        public Contact_Page_Service(IUnit_Of_Work_Repo unitOfWork, IContact_Page_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
