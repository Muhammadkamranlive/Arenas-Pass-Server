using Microsoft.AspNetCore.Mvc;

namespace API.API.MediaController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UploadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [CustomAuthorize("Write")]
        public IActionResult UploadImage(IFormFile file)
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
    }
}
