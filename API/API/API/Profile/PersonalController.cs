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
    public class PersonalController : ParentController<Personal, PersonalModel>
    {
        private readonly IProfile_Service _Service;
        private readonly IAuthManager    _authManager;
        private readonly IEmergencyContact_Service _emergencyContactService;
        private readonly IJobExperience_Service _jobExperienceService;
        private readonly IProfessionalLicense_Service _professionalLicenseService;
        private readonly IDependent_Service      _dependentService;
        private readonly IAttachments_Service   _attachments_Service;
        private readonly IEducations_Service    _educations_Service;
        private readonly IGeneralTask_Service   _generalTask_Service;
        private readonly INotifications_Service _notifications_Service;
        private readonly ICaseManagment_Service _caseManagment_Service;
        private readonly ITraining_Service      _training_Service;
      
        public PersonalController
        (
            IProfile_Service               service,
            IMapper                        mapper,
            IAuthManager                   authManager,
            IEmergencyContact_Service      emergencyContact_Service,
            IJobExperience_Service         jobExperience,
            IAttachments_Service           attachments_Service,
            IProfessionalLicense_Service   professionalLicense_Service,
            IDependent_Service             dependent_Service,
            IEducations_Service            educations_Service,
            IGeneralTask_Service           generalTask_Service,
            INotifications_Service         notifications_Service,
            ICaseManagment_Service         caseManagment_Service,
            ITraining_Service              training_Service
        ) : base(service, mapper)
        {
            _Service                    = service;
            _authManager                = authManager;
            _emergencyContactService    = emergencyContact_Service;
            _jobExperienceService       = jobExperience;
            _attachments_Service        = attachments_Service;
            _professionalLicenseService = professionalLicense_Service;
            _dependentService           = dependent_Service;
            _educations_Service         = educations_Service;
            _generalTask_Service        = generalTask_Service;
            _notifications_Service      = notifications_Service;
            _caseManagment_Service      = caseManagment_Service;
            _training_Service           = training_Service;

        }


        [HttpGet]
        [Route("ProfileScore")]
        [CustomAuthorize("Read")]
        public async Task<int> ProfileScore(string userid)
        {
            try
            {
                int score = 0;
                IList<Dependent> dep            = await _dependentService.Find(x => x.userId == userid);
                IList<Education> edu            = await _educations_Service.Find(x=>x.userId== userid);
                IList<EmergencyContacts> em     = await _emergencyContactService.Find(x=>x.userId== userid);
                IList<JobExperience> job        = await _jobExperienceService.Find(x => x.userId == userid);
                IList<Attachments> att          = await _attachments_Service.Find(x => x.userId == userid);
                IList<ProfessionalLicense> prof = await _professionalLicenseService.Find(x=>x.userId==userid);
                IList<Personal> per             = await _Service.Find(x=>x.UserId==userid);

                if (dep.Count!=0)
                    score += 10;

                if (prof.Count!=0)
                    score += 20;

                if (edu.Count!=0)
                    score += 10;
                if (per.Count != 0)
                    score += 10;

                if (att.Count!=0)
                    score += 20;

                if (em.Count!=0)
                    score += 20;
                if (job.Count != 0)
                    score += 10;
                return score;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("Profile")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Profile(string userid)
        {
            try
            {
                 IList <Personal> per = await _Service.Find(x => x.UserId == userid);
                 return Ok(per.LastOrDefault());
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }


        [HttpGet]
        [Route("EmergengyContacts")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> EmergengyContacts(string userid)
        {
            try
            {
                IList<EmergencyContacts> per = await _emergencyContactService.Find(x => x.userId == userid);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("Dependetns")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Dependetns(string userid)
        {
            try
            {
                IList<Dependent> per = await _dependentService.Find(x => x.userId == userid);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("Education")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Education(string userid)
        {
            try
            {
                IList<Education> per = await _educations_Service.Find(x => x.userId == userid);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("JobExperience")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> JobExperience(string userid)
        {
            try
            {
                IList<JobExperience> per = await _jobExperienceService.Find(x => x.userId == userid);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("ProfessionalLicense")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> ProfessionalLicense(string userid)
        {
            try
            {
                IList<ProfessionalLicense> per = await _professionalLicenseService.Find(x => x.userId == userid);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("Attachment")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Attachment(string userid)
        {
            try
            {
                IList<Attachments> per = await _attachments_Service.Find(x => x.userId == userid);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("Tasks")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Tasks(string userid)
        {
            try
            {
                IList<GENERALTASK> per = await _generalTask_Service.Find(x => x.UserId == userid);
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }


        [HttpGet]
        [Route("Notifications")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Notifications(string userid)
        {
            try
            {
                IList<NOTIFICATIONS> per = await _notifications_Service.Find(x => x.UserId == userid);
                per = per.OrderByDescending(x => x.Timestamp).ToList();
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }


     

        [HttpGet]
        [Route("CasesForAgents")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> CasesForAgents(string userid)
        {
            try
            {
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();
                IList<Case> cases = (IList<Case>)await _caseManagment_Service.GetAll();

                var result = cases
                    .Join(
                        users,
                        c => c.CustomerId,
                        u => u.Id,
                        (c, u) => new { Case = c, Customer = u }
                    )
                    .Join(
                        users,
                        c => c.Case.AgentId,
                        u => u.Id,
                        (c, u) => new { c.Case, c.Customer, Agent = u }
                    )
                    .Where(x => x.Customer.Id == userid || x.Case.AgentId == userid)
                    .Select(x => new
                    {
                        x.Case.Id,
                        x.Case.Title,
                        x.Case.Description,
                        x.Case.Status,
                        x.Case.CreatedAt,
                        CustomerFirstName = x.Customer.FirstName,
                        CustomerLastName = x.Customer.LastName,
                        CustomerEmail = x.Customer.Email,
                        CustomerRoles = x.Customer.Roles,
                        x.Case.CustomerId,
                        AssignedAgentId = x.Agent.Id,
                        AssignedAgentFirstName = x.Agent.FirstName,
                        AssignedAgentLastName = x.Agent.LastName,
                        AssignedAgentEmail = x.Agent.Email,
                        AssignedAgentRoles = x.Agent.Roles
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.InnerException?.Message);
            }
        }


        [HttpGet]
        [Route("AllCases")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> AllCases()
        {
            try
            {
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();
                IList<Case> cases = (IList<Case>)await _caseManagment_Service.GetAll();

                var result = cases
                    .Join(
                        users,
                        c => c.CustomerId,
                        u => u.Id,
                        (c, u) => new { Case = c, Customer = u }
                    )
                    .Join(
                        users,
                        c => c.Case.AgentId,
                        u => u.Id,
                        (c, u) => new { c.Case, c.Customer, Agent = u }
                    )
                    
                    .Select(x => new
                    {
                        x.Case.Id,
                        x.Case.Title,
                        x.Case.Description,
                        x.Case.Status,
                        x.Case.CreatedAt,
                        CustomerFirstName = x.Customer.FirstName,
                        CustomerLastName = x.Customer.LastName,
                        CustomerEmail = x.Customer.Email,
                        CustomerRoles = x.Customer.Roles,
                        x.Case.CustomerId,
                        AssignedAgentId = x.Agent.Id,
                        AssignedAgentFirstName = x.Agent.FirstName,
                        AssignedAgentLastName = x.Agent.LastName,
                        AssignedAgentEmail = x.Agent.Email,
                        AssignedAgentRoles = x.Agent.Roles
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.InnerException?.Message);
            }
        }


        [HttpGet]
        [Route("Trainings")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Trainings(string userid)
        {
            try
            {
              var train =  await _training_Service.Find(x=>x.userId== userid);

                return Ok(train);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.InnerException?.Message);
            }
        }


    }
}
