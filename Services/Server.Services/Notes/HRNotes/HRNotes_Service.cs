using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class HRNotes_Service:Base_Service<HRNotes>, IHRNotes_Service
    {
        public HRNotes_Service(IUnit_Of_Work_Repo unitOfWork, IHRNotes_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
