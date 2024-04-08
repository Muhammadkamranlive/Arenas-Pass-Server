using Microsoft.AspNetCore.Mvc;

namespace API.API.DocumentManagment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHostEnvironment    _env;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="env"></param>
        public ImagesController
        (
            IWebHostEnvironment hostingEnvironment,
            IHostEnvironment env
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _env = env;
        }

        [HttpPost]
        [CustomAuthorize("Write")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file selected or invalid file.");
                }

                // Generate a unique file name for the uploaded image
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine("UploadedImages", fileName);

                // Get the Azure-specific web root path
                string webRootPath = _hostingEnvironment.ContentRootPath;

                // Construct the full path
                string fullPath = Path.Combine(webRootPath, filePath);

                // Create the directory if it doesn't exist
                string directoryPath = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Save the file to the server
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Return the URL to access the uploaded image or an empty string if the file is not found
                string imageUrl = System.IO.File.Exists(fullPath) ? $"/{filePath}" : string.Empty;

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("images/{imageName}")]
        
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine(_env.ContentRootPath, "upload", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }
            var extension = Path.GetExtension(imageName).ToLower();
            string contentType;

            switch (extension)
            {
                case ".pdf":
                contentType = "application/pdf";
                break;
                case ".webp":
                contentType = "image/webp";
                break;
                case ".jpg":
                case ".jpeg":
                contentType = "image/jpeg";
                break;
                case ".png":
                contentType = "image/png";
                break;
                case ".gif":
                contentType = "image/gif";
                break;
                default:
                contentType = "application/octet-stream";
                break;
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, contentType);
        }

    
    }
}
