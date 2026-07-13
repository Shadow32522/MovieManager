using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Entities;

namespace MovieManager.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ActorsController : ControllerBase
    {
        private readonly IGenericService<ActorModel> _service;

        public ActorsController(IGenericService<ActorModel> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ActorModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var actors = await _service.GetAllAsync(cancellationToken);
            return Ok(actors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActorModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId(int id, CancellationToken cancellationToken)
        {
            var actors = await _service.GetByIdAsync(id, cancellationToken);
            if (actors == null)
            {
                return NotFound(new { message = "Actor not found.", id });
            }
            return Ok(actors);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ActorModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ActorModel actors, CancellationToken cancellationToken)
        {
            if (actors == null)
            {
                return BadRequest(new { message = "Actor data cannot be null." });
            }
            var createdActors = await _service.CreateAsync(actors, cancellationToken);
            return CreatedAtAction(nameof(GetId), new { id = createdActors.Id }, createdActors);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ActorModel actors, CancellationToken cancellationToken)
        {
            if (actors == null || id != actors.Id)
            {
                return BadRequest(new { message = "Invalid actor data. The ID in the URL does not match the ID in the request body." });
            }
            var existingActors = await _service.GetByIdAsync(id, cancellationToken);
            if (existingActors == null)
            {
                return NotFound(new { message = "Actor not found.", id });
            }
            await _service.UpdateAsync(actors, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var existingActors = await _service.GetByIdAsync(id, cancellationToken);
            if (existingActors == null)
            {
                return NotFound(new { message = "Actor not found.", id });
            }
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
