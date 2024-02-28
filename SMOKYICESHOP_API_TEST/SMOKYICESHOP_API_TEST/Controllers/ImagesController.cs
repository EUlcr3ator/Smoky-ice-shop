using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ImagesModel _model;

        public ImagesController(ImagesModel model)
        {
            _model = model;
        }

        [HttpGet("{imageId}")]
        public IActionResult GetImage(Guid imageId)
        {
            try
            {
                byte[] image = _model.GetImage(imageId);
                return File(image, "image/webp");
            }
            catch(InvalidOperationException)
            {
                return NotFound("Image not found");
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Roles = "Admin")]
        public ActionResult<Guid> PostImage([FromForm] IFormFile image)
        {
            Guid imageId;

            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                imageId = _model.AddImage(fileBytes);
            }
            return CreatedAtAction(nameof(GetImage), new { imageId = imageId }, imageId);
        }

        [HttpHead("remove-not-used")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAllImages()
        {
            _model.DeleteNotUsed();
            return Ok();
        }
    }
}
