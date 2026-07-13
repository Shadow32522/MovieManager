using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Entities;

namespace MovieManager.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GenresController : ControllerBase
    {
        private readonly IGenericService<GenreModel> _service;

        public GenresController(IGenericService<GenreModel> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GenreModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var genres = await _service.GetAllAsync(cancellationToken);
            return Ok(genres);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GenreModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId(int id, CancellationToken cancellationToken)
        {
            var genre = await _service.GetByIdAsync(id, cancellationToken);
            if (genre == null)
            {
                return NotFound(new { message = "Genre not found.", id });
            }
            return Ok(genre);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GenreModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] GenreModel genre, CancellationToken cancellationToken)
        {
            if (genre == null)
            {
                return BadRequest(new { message = "Genre data cannot be null." });
            }
            var createdGenre = await _service.CreateAsync(genre, cancellationToken);
            return CreatedAtAction(nameof(GetId), new { id = createdGenre.Id }, createdGenre);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] GenreModel genre, CancellationToken cancellationToken)
        {
            if (genre == null || id != genre.Id)
            {
                return BadRequest(new { message = "Invalid genre data. The ID in the URL does not match the ID in the request body." });
            }
            var existingDirector = await _service.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                return NotFound(new { message = "Genre not found.", id });
            }
            await _service.UpdateAsync(genre, cancellationToken);
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
                return NotFound(new { message = "Genre not found.", id });
            }
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
