using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.ContactManagment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ParentController<CONTACTDETAILS, ContactDetailModel>
    {
        private readonly IContactDetails_Service _Service;
        private readonly IAuthManager           _authManager;
        public ContactController
        (
            IContactDetails_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service     = service;
            _authManager = authManager;
        }
    }
}
