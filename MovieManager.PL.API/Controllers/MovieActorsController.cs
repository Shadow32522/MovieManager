using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MovieActorsController : ControllerBase
    {
        private readonly IMovieActorService _service;

        public MovieActorsController(IMovieActorService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{movieId}/{actorId}")]
        [ProducesResponseType(typeof(MovieActorModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIds(int movieId, int actorId)
        {
            if (movieId <= 0)
            {
                return BadRequest(new { message = "Movie ID must be greater than 0.", movieId });
            }

            if (actorId <= 0)
            {
                return BadRequest(new { message = "Actor ID must be greater than 0.", actorId });
            }

            var movieActor = await _service.GetByIdsAsync(movieId, actorId);
            if (movieActor == null)
            {
                return NotFound(new { message = "Movie-actor relationship not found.", movieId, actorId });
            }

            return Ok(movieActor);
        }

        [HttpGet("movie/{movieId}")]
        [ProducesResponseType(typeof(IEnumerable<MovieActorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByMovieId(int movieId)
        {
            if (movieId <= 0)
            {
                return BadRequest(new { message = "Movie ID must be greater than 0.", movieId });
            }

            var movieActors = await _service.GetByMovieIdAsync(movieId);
            return Ok(movieActors);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MovieActorModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddActorToMovie([FromBody] MovieActorModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Movie-actor data cannot be null." });
            }

            if (model.MovieId <= 0)
            {
                return BadRequest(new { message = "Movie ID must be greater than 0." });
            }

            if (model.ActorId <= 0)
            {
                return BadRequest(new { message = "Actor ID must be greater than 0." });
            }

            var createdMovieActor = await _service.AddActorToMovieAsync(model);
            return CreatedAtAction(nameof(GetByIds),
                new { movieId = createdMovieActor.MovieId, actorId = createdMovieActor.ActorId },
                createdMovieActor);
        }

        [HttpDelete("{movieId}/{actorId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveActorFromMovie(int movieId, int actorId)
        {
            if (movieId <= 0)
            {
                return BadRequest(new { message = "Movie ID must be greater than 0.", movieId });
            }

            if (actorId <= 0)
            {
                return BadRequest(new { message = "Actor ID must be greater than 0.", actorId });
            }

            var success = await _service.RemoveActorFromMovieAsync(movieId, actorId);
            if (!success)
            {
                return NotFound(new { message = "Movie-actor relationship not found.", movieId, actorId });
            }

            return NoContent();
        }
    }
}
