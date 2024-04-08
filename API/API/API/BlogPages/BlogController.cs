using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.BlogPages
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ParentController<BlogPage, BlogModel>
    {
        private readonly IBlog_Service _Service;
        private readonly IWebPage_Service _ServiceWeb;
        private readonly IAuthManager _authManager;
        public BlogController
        (
            IBlog_Service service,
            IMapper mapper,
            IAuthManager authManager,
            IWebPage_Service ervice
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
            _ServiceWeb = ervice;
        }

        [HttpGet]
        [Route("GetAllPages")]
        public  async Task<IActionResult> GetAllPages()
        {

            try
            {
                var result = await _ServiceWeb.GetAll();
               
                return Ok(result.OrderBy(x=>x.CreatedAt));
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }


        [HttpGet]
        [Route("GetAllPost")]
        public  async Task<IActionResult> GetAllPost()
        {

            try
            {
                var result = await genericService.GetAll();

                return Ok(result.OrderBy(x => x.CreatedAt));
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

        [HttpPut]
        [Route("UpdatePost")]
        public virtual async Task<IActionResult> UpdatePost(BlogPage blogPage)
        {

            try
            {
                 genericService.UpdateRecord(blogPage);
                var update = await _Service.CompleteAync();
                if (update == 1)
                {
                    var message = "Data Updated Successfully for Blog Post";
                    return Content($"{{ \"message\": \"{message}\" }}", "application/json");
                }

                return Ok(update);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }


        [HttpPut]
        [Route("UpdatePage")]
        public virtual async Task<IActionResult> UpdatePage(WebPages blogPage)
        {

            try
            {
                _ServiceWeb.UpdateRecord(blogPage);
                var update = await _ServiceWeb.CompleteAync();
                if (update == 1)
                {
                    var message = "Data Updated Successfully for Web Page";
                    return Content($"{{ \"message\": \"{message}\" }}", "application/json");
                }

                return Ok(update);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }




        [HttpGet]
        [Route("GetBlogAll")]
        public  async Task<IActionResult> GetBlogAll()
        {

            try
            {
                var result = await _Service.GetAll();
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();
                var clientResult = result.
                    Join(users,
                         (b) => new { Id = b.userId },
                         (u) => new { Id = u.Id },
                         (b, u) => new { b, u }
                      )

                    .Select
                 (x => new
                 {
                     x.b.Id,
                     x.b.Title,
                     x.b.Description,
                     x.b.MetaDescription,
                     x.b.image,
                     CreatedAt = x.b.CreatedAt.ToLongDateString(),
                     name = x.u.FirstName + "" + x.u.LastName,
                     userImage = x.u.Image,
                 })
                .OrderBy(x => x.CreatedAt)
                .ToList();
                return Ok(clientResult);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }

        }

        [HttpGet]
        [Route("getDetail")]
        public async Task<IActionResult> getDetail(Guid id)
        {

            try
            {
                var result = await _Service.GetAll();
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();
                var clientResult = result.
                    Join(users,
                         (b) => new { Id = b.userId },
                         (u) => new { Id = u.Id },
                         (b, u) => new { b, u }
                      )
                    .Where(x => x.b.Id == id)
                  .Select
                  (x => new
                  {
                      x.b.Id,
                      x.b.Title,
                      x.b.Description,
                      x.b.MetaDescription,
                      x.b.image,
                      CreatedAt = x.b.CreatedAt.ToLongDateString(),
                      name = x.u.FirstName + "" + x.u.LastName,
                      userImage = x.u.Image,
                  })
                 .FirstOrDefault();
                return Ok(clientResult);


            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }

        }



    }
}
