using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Zoom_Meeting_Service:Base_Service<ZoomMeetings>, IZoom_Meeting_Service
    {
        public Zoom_Meeting_Service(IUnit_Of_Work_Repo unitOfWork, IZoom_Meeting_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
