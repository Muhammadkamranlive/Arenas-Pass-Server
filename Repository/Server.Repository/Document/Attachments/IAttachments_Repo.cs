using Server.Core;
using Server.Domain;
using System.Collections;

namespace Server.Repository
{
    public interface IAttachments_Repo:IRepo<Attachments>
    {
        Task<IEnumerable> GetAllAttachment(int Id);
    }
}
