using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

        [HttpGet]
        [Route("TenantFiles")]
        //[CustomAuthorize("Read")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperUser")]
        public async Task<IActionResult> TenantFiles(int id)
        {
            try
            {
                IList<Attachments> per = (IList<Attachments>)await _Service.GetAllAttachment(id);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }
    }
}
