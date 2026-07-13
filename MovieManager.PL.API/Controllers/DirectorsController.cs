using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Entities;

namespace MovieManager.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DirectorsController : ControllerBase
    {
        private readonly IGenericService<DirectorModel> _service;

        public DirectorsController(IGenericService<DirectorModel> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DirectorModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var directors = await _service.GetAllAsync(cancellationToken);
            return Ok(directors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DirectorModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId(int id, CancellationToken cancellationToken)
        {
            var director = await _service.GetByIdAsync(id, cancellationToken);
            if (director == null)
            {
                return NotFound(new { message = "Director not found.", id });
            }
            return Ok(director);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DirectorModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] DirectorModel director, CancellationToken cancellationToken)
        {
            if (director == null)
            {
                return BadRequest(new { message = "Director data cannot be null." });
            }
            var createdDirector = await _service.CreateAsync(director, cancellationToken);
            return CreatedAtAction(nameof(GetId), new { id = createdDirector.Id }, createdDirector);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] DirectorModel director, CancellationToken cancellationToken)
        {
            if (director == null || id != director.Id)
            {
                return BadRequest(new { message = "Invalid director data. The ID in the URL does not match the ID in the request body." });
            }
            var existingDirector = await _service.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                return NotFound(new { message = "Director not found.", id });
            }
            await _service.UpdateAsync(director, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var existingDirector = await _service.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                return NotFound(new { message = "Director not found.", id });
            }
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
