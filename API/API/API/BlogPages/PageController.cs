using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.BlogPages
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ParentController<WebPages, WebPagesModel>
    {
        private readonly IWeb_Page_Service _Service;
        private readonly IAuth_Manager_Service     _authManager;
        private readonly IMapper          _mapper;
        public PageController
        (
            IWeb_Page_Service service,
            IMapper mapper,
            IAuth_Manager_Service authManager
        ) : base(service, mapper)
        {
            _Service     = service;
            _mapper      = mapper;
            _authManager = authManager;
        }



        [HttpGet]
        [Route("GetAllPages")]
        public  async Task<IActionResult> GetAllPages()
        {

            try
            {
                var result       = await genericService.GetAll();
                var clientResult = _mapper.Map<IList<WebPagesModel>>(result);
                return Ok(clientResult);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpGet()]
        [Route("GetByPageId")]
        public async Task<IActionResult> GetByPageId(Guid id)
        {
            try
            {
                var result       = await genericService.Get(id);
                var clientResult = _mapper.Map<WebPagesModel>(result);
                return Ok(clientResult);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

    }
}
