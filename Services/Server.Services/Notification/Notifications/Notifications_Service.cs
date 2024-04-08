using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Notifications_Service:Base_Service<NOTIFICATIONS>, INotifications_Service
    {
        public Notifications_Service(IUnitOfWork unitOfWork, INotifications_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
