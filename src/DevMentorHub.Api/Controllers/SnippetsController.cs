using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Application.Queries;

namespace DevMentorHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Authorize]
    public class SnippetsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SnippetsController(IMediator mediator) { _mediator = mediator; }

        [HttpPost("projects/{projectId:guid}/snippets")]
        public async Task<ActionResult<SnippetDto>> Create(Guid projectId, [FromBody] CreateSnippetCommand cmd)
        {
            cmd.ProjectId = projectId;
            var dto = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { snippetId = dto.Id }, dto);
        }

        [HttpGet("snippets/{snippetId:guid}")]
        public async Task<ActionResult<SnippetDto>> GetById(Guid snippetId)
        {
            var dto = await _mediator.Send(new GetSnippetQuery { SnippetId = snippetId });
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPut("snippets/{snippetId:guid}")]
        public async Task<IActionResult> Update(Guid snippetId, [FromBody] UpdateSnippetCommand cmd)
        {
            if (cmd.Id == Guid.Empty) cmd.Id = snippetId;
            await _mediator.Send(cmd);
            return NoContent();
        }

        [HttpDelete("snippets/{snippetId:guid}")]
        public async Task<IActionResult> Delete(Guid snippetId)
        {
            await _mediator.Send(new DeleteSnippetCommand { Id = snippetId });
            return NoContent();
        }
    }
}