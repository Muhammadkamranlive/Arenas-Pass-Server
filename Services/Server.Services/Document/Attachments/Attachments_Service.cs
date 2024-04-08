using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Attachments_Service:Base_Service<Attachments>, IAttachments_Service
    {
        public Attachments_Service(IUnitOfWork unitOfWork, IAttachments_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
