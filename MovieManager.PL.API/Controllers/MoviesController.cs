using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Entities;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace MovieManager.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly IGenericService<MovieModel> _service;

        public MoviesController(IGenericService<MovieModel> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var movies = await _service.GetAllAsync(cancellationToken);
            return Ok(movies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId(int id, CancellationToken cancellationToken)
        {
            var movie = await _service.GetByIdAsync(id, cancellationToken);
            if (movie == null)
            {
                return NotFound(new { message = "Movie not found.", id });
            }
            return Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MovieModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] MovieModel movie, CancellationToken cancellationToken)
        {
            if (movie == null)
            {
                return BadRequest("The provided data is null.");
            }
            var createdMovie = await _service.CreateAsync(movie, cancellationToken);
            return CreatedAtAction(nameof(GetId), new { id = createdMovie.Id }, createdMovie);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] MovieModel movie, CancellationToken cancellationToken)
        {
            if (movie == null || id != movie.Id)
            {
                return BadRequest(new { message = "Invalid movie data. The ID in the URL does not match the ID in the request body." });
            }
            var existingDirector = await _service.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                return NotFound(new { message = "Movie not found.", id });
            }
            await _service.UpdateAsync(movie, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var existingDirector = await _service.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                return NotFound(new { message = "Movie not found.", id });
            }
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
