using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Profile
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalLicenseController : ParentController<ProfessionalLicense,ProfessionalLicenseModel>
    {
        private readonly IProfessional_License_Service _Service;
        private readonly IAuth_Manager_Service                _authManager;
        public ProfessionalLicenseController
        (
            IProfessional_License_Service service,
            IMapper                      mapper,
            IAuth_Manager_Service                 authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
