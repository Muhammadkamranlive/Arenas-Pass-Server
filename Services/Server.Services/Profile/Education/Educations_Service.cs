using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Educations_Service:Base_Service<Education>, IEducations_Service
    {
        public Educations_Service(IUnit_Of_Work_Repo unitOfWork, IEducation_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
