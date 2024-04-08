using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Logs_Service:Base_Service<AdminLogs>,ILog_Service
    {
        public Logs_Service(IUnitOfWork unitOfWork,ILog_Repo log_Repo):base(unitOfWork,log_Repo)
        {

        }
    }
}
