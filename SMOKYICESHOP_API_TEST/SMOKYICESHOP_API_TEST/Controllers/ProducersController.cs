using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;
using System.Data;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly ProducersModel _producersModel;

        public ProducersController(ProducersModel producersModel)
        {
            _producersModel = producersModel;
        }

        [HttpGet("category/{categoryName}")]
        public ActionResult<IEnumerable<ProducerDTO>> GetProducersByCategory(string categoryName)
        {
            return Ok(_producersModel.GetAllProducers(categoryName));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<ProducerDTO>> GetProducers()
        {
            return Ok(_producersModel.GetAllProducers());
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult PostProducer([FromBody] CreateProducerDTO producer)
        {
            Guid id = _producersModel.AddProducer(producer);
            ProducerDTO dto = _producersModel.GetProducer(id);
            return CreatedAtAction(nameof(GetProducer), new { producerId = dto.Id }, dto);
        }

        [HttpHead("configure-categories")]
        [Authorize(Roles = "Admin")]
        public IActionResult ConfigureProducersCategories()
        {
            _producersModel.ConfigureProducersCategories();
            return Ok();
        }

        [HttpGet("{producerId}")]
        public ActionResult<ProducerDTO> GetProducer(Guid producerId)
        {
            try
            {
                return Ok(_producersModel.GetProducer(producerId));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}

