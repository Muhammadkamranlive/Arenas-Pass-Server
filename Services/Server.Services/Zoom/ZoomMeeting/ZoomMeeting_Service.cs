using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class ZoomMeeting_Service:Base_Service<ZoomMeetings>, IZoomMeeting_Service
    {
        public ZoomMeeting_Service(IUnitOfWork unitOfWork, IZoomMeting_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
