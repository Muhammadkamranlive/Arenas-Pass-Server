using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Blog_Service:Base_Service<BlogPage>,IBlog_Service
    {
        public Blog_Service(IUnitOfWork unitOfWork, IBlog_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
