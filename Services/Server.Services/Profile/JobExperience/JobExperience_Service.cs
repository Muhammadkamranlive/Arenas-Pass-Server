using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class JobExperience_Service:Base_Service<JobExperience>, IJobExperience_Service
    {
        public JobExperience_Service(IUnit_Of_Work_Repo unitOfWork, IJob_Experience_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
