using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.CustomerCare
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ParentController<Case, CaseModel>
    {
        private readonly ICaseManagment_Service _Service;
        private readonly ICaseComments_Service  _cService;
        private readonly IAuthManager           _authManager;
        public CaseController
        (
            ICaseManagment_Service service,
            IMapper mapper,
            IAuthManager authManager,
            ICaseComments_Service c
        ) : base(service, mapper)
        {
            _Service     = service;
            _authManager = authManager;
            _cService    = c;
        }





        [HttpGet]
        [Route("UnAssignedCases")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> UnAssignedCases()
        {
            try
            {
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();
                IList<Case> cases          = (IList<Case>)await _Service.GetAll();

                var result = cases
                    .Join
                    (
                        users,
                        c => c.CustomerId,
                        u => u.Id,
                        (c, u) => new { Case = c, Customer = u }
                    )

                    .Where(x=>x.Case.AgentId=="A")
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
                        AssignedAgentId = "",
                        AssignedAgentFirstName = "",
                        AssignedAgentLastName = "",
                        AssignedAgentEmail = "",
                        AssignedAgentRoles = "",
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
        [Route("GetCaseComments")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> GetCaseComments(Guid id)
        {

            try
            {
                var comments               = await _cService.GetAll();
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();


                var list = comments.Join
                    (
                      users,
                         (co)    => new { co.UserId },
                         (u)     => new { UserId = u.Id },
                         (co, u) => new { co, u }
                    )
                    .Where
                    (
                     x => x.co.CaseId == id
                    )
                    .Select(x => new
                    {
                        x.co.Id,
                        x.co.Text,
                        x.co.CreatedAt,
                        name = x.u.FirstName + " " + x.u.LastName,
                        x.u.Email,
                        x.u.Roles,
                        x.u.Image,

                    })
                    .OrderBy(x => x.CreatedAt)
                    .ToList();





                return Ok(list);


            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }




    }
}
