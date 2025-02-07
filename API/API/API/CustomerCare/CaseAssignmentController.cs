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
    public class CaseAssignmentController : ParentController<CaseComment, CaseCommentModel>
    {
        private readonly ICase_Comments_Service  _Service;
        private readonly IAuth_Manager_Service           _authManager;
        private readonly ICase_Managment_Service _caseManagment_Service;
        public CaseAssignmentController
        (
            ICase_Comments_Service service,
            IMapper mapper,
            IAuth_Manager_Service authManager,
            ICase_Managment_Service caseManagment_Service
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
            _caseManagment_Service= caseManagment_Service;
        }
        [HttpGet]
        [Route("GetCaseComments")]
        [CustomAuthorize("Read")]
        public  async Task<IActionResult> GetCaseComments(Guid id)
        {

            try
            {
                var comments = await _Service.GetAll();
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();


                var list = comments.Join
                    (
                      users,
                      (co) => new { co.UserId },
                      (u) => new { UserId = u.Id },
                      (co, u) => new { co, u }
                    )
                   .Where(x => x.co.CaseId == id)
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
                    .OrderBy(x=>x.CreatedAt)
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
