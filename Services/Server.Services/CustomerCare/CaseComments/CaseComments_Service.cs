using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class CaseComments_Service:Base_Service<CaseComment>, ICaseComments_Service
    {
        public CaseComments_Service(IUnitOfWork unitOfWork, ICaseCommetns_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
