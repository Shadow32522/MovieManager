using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Repositories.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {
        private readonly IGenericService<ReviewModel> _service;

        public ReviewsController(IGenericService<ReviewModel> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReviewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var reviews = await _service.GetAllAsync(cancellationToken);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReviewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId(int id, CancellationToken cancellationToken)
        {
            var review = await _service.GetByIdAsync(id, cancellationToken);
            if (review == null)
            {
                return NotFound(new { message = "Review not found.", id });
            }
            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReviewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ReviewModel review, CancellationToken cancellationToken)
        {
            if (review == null)
            {
                return BadRequest(new { message = "Review data cannot be null." });
            }
            var createdReview = await _service.CreateAsync(review, cancellationToken);
            return CreatedAtAction(nameof(GetId), new { id = createdReview.Id }, createdReview);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ReviewModel review, CancellationToken cancellationToken)
        {
            if (review == null || id != review.Id)
            {
                return BadRequest(new { message = "Invalid review data. The ID in the URL does not match the ID in the request body." });
            }
            var existingReview = await _service.GetByIdAsync(id, cancellationToken);
            if (existingReview == null)
            {
                return NotFound(new { message = "Review not found.", id });
            }
            await _service.UpdateAsync(review, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var existingReview = await _service.GetByIdAsync(id, cancellationToken);
            if (existingReview == null)
            {
                return NotFound(new { message = "Review not found.", id });
            }
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
