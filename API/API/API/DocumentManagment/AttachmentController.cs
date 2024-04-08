using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.DocumentManagment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ParentController<Attachments, AttachmentModel>
    {
        private readonly IAttachments_Service     _Service;
        private readonly IAuthManager             _authManager;
        public AttachmentController
        (
            IAttachments_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service     = service;
            _authManager = authManager;
        }
    }
}
