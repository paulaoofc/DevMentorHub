using DevMentorHub.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.Queries;

namespace DevMentorHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewsController(IMediator mediator) { _mediator = mediator; }

        [HttpPost("snippets/{snippetId:guid}/reviews")]
        public async Task<ActionResult<CodeReviewDto>> Create(Guid snippetId, [FromBody] CreateCodeReviewCommand cmd)
        {
            cmd.SnippetId = snippetId;
            var dto = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { reviewId = dto.Id }, dto);
        }

        [HttpGet("snippets/{snippetId:guid}/reviews")]
        public async Task<ActionResult<IEnumerable<CodeReviewSummaryDto>>> GetBySnippet(Guid snippetId)
        {
            var list = await _mediator.Send(new GetSnippetReviewsQuery { SnippetId = snippetId });
            return Ok(list);
        }

        [HttpGet("reviews/{reviewId:guid}")]
        public async Task<ActionResult<CodeReviewDto>> GetById(Guid reviewId)
        {
            var dto = await _mediator.Send(new GetCodeReviewByIdQuery { ReviewId = reviewId });
            if (dto is null) return NotFound();
            return Ok(dto);
        }
    }
}