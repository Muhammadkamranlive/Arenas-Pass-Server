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
    public class ContactPageController : ParentController<ContactPage, ContactModel>
    {
        private readonly IContactPage_Service _pagesService;
        private readonly IMapper              _mapper;
        public ContactPageController(IContactPage_Service service, IMapper mapper) : base(service, mapper)
        {
            _pagesService = service;
            _mapper       = mapper;
        }


        [HttpPost]
        [Route("AddContact")]
        public  async Task<IActionResult> AddContact(ContactModel autoMapperEntity)
        {


            try
            {
                var clientData = _mapper.Map<ContactPage>(autoMapperEntity);
                await genericService.InsertAsync(clientData);
                await genericService.CompleteAync();
                var message = "Thank you for reaching out. We have received your message and will address your inquiry promptly.";
                return Content($"{{ \"message\": \"{message}\" }}", "application/json");

            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }

        }
    }
}
