using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Designation_Service:Base_Service<Designations>, IDesignation_Service
    {
        public Designation_Service(IUnit_Of_Work_Repo unitOfWork, IDesignation_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
