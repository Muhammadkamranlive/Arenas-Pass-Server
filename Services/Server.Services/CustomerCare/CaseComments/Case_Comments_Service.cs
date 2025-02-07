using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Case_Comments_Service:Base_Service<CaseComment>, ICase_Comments_Service
    {
        public Case_Comments_Service(IUnit_Of_Work_Repo unitOfWork, ICaseCommetns_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
