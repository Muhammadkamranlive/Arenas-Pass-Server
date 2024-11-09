using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardsController : ControllerBase
    {
        #region Contructor
        private readonly IGift_Cards_Service _Gift_Service;
        public GiftCardsController
        (
          IGift_Cards_Service gcards_Service    
        )
        {
            _Gift_Service = gcards_Service;
        }
        #endregion




    }
}
