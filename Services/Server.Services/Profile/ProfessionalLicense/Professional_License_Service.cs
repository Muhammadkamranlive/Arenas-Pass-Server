using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Professional_License_Service:Base_Service<ProfessionalLicense>, IProfessional_License_Service
    {
        public Professional_License_Service(IUnit_Of_Work_Repo unitOfWork, IProfessional_License_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
