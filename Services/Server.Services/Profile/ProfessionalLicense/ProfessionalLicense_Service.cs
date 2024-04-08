using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class ProfessionalLicense_Service:Base_Service<ProfessionalLicense>, IProfessionalLicense_Service
    {
        public ProfessionalLicense_Service(IUnitOfWork unitOfWork, IProfessionalLicense_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
