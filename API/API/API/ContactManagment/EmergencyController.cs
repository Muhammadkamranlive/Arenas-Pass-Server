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
    public class EmergencyController :ParentController<EmergencyContacts, EmergencyContactModel>
    {
        private readonly IEmergencyContact_Service _Service;
        private readonly IAuthManager            _authManager;
        public EmergencyController
        (
            IEmergencyContact_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service     = service;
            _authManager = authManager;
        }


    }
}
