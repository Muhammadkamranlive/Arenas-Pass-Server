using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Candidate_Service:Base_Service<CandidateInfo>, ICandidate_Service
    {
        public Candidate_Service(IUnitOfWork unitOfWork, ICandidate_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
