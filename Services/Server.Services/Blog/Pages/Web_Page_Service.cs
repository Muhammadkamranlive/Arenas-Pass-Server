using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Web_Page_Service:Base_Service<WebPages>,IWeb_Page_Service
    {
        public Web_Page_Service(IUnit_Of_Work_Repo unitOfWork, IWeb_Page_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
