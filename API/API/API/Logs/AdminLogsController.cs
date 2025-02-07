using Server.Domain;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.HRAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLogsController : ControllerBase
    {
        private readonly ILog_Service _service;
        public AdminLogsController(ILog_Service service)
        {
            _service = service;
        }


        [HttpGet]
        //[CustomAuthorize("Read")]
        public async Task<IActionResult> Logs()
        {
            try
            {
                IList<AdminLogs> per = (IList<AdminLogs>)await _service.GetAll();
                per = per.OrderBy(x => x.Timestamp).ToList();
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }
    }
}
