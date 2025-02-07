using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;
using System.Collections;

namespace Server.Services
{
    public class Attachments_Service:Base_Service<Attachments>, IAttachments_Service
    {
        private readonly IAttachments_Repo iRepo;
        public Attachments_Service(IUnit_Of_Work_Repo unitOfWork, IAttachments_Repo _Repo) : base(unitOfWork, _Repo)
        {
            iRepo = _Repo;
        }

        public async Task<IEnumerable> GetAllAttachment(int Id)
        {
            try
            {
                return await iRepo.GetAllAttachment(Id);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
