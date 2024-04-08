using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class WebPage_Service:Base_Service<WebPages>,IWebPage_Service
    {
        public WebPage_Service(IUnitOfWork unitOfWork, IPage_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
