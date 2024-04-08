using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class PasswordReset_Service : Base_Service<PasswordResetDomain>, IPasswordReset_Service
    {
        public PasswordReset_Service(IUnitOfWork unitOfWork, IPasswordReset_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
