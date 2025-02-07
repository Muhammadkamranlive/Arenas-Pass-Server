using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Password_Reset_Service : Base_Service<PasswordResetDomain>, IPassword_Reset_Service
    {
        public Password_Reset_Service(IUnit_Of_Work_Repo unitOfWork, IPassword_Reset_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
