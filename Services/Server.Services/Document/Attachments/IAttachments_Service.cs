using Server.Core;
using Server.Domain;
using System.Collections;

namespace Server.Services
{
    public interface IAttachments_Service:IBase_Service<Attachments>
    {
        Task<IEnumerable> GetAllAttachment(int Id);
    }
}
