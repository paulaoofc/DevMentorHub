using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Application.Queries;

namespace DevMentorHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectsController(IMediator mediator) { _mediator = mediator; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetMyProjects()
        {
            var result = await _mediator.Send(new GetUserProjectsQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectCommand cmd)
        {
            var dto = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { projectId = dto.Id }, dto);
        }

        [HttpGet("{projectId:guid}")]
        public async Task<ActionResult<ProjectWithSnippetsDto>> GetById(Guid projectId)
        {
            var dto = await _mediator.Send(new GetProjectWithSnippetsQuery { ProjectId = projectId });
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPut("{projectId:guid}")]
        public async Task<IActionResult> Update(Guid projectId, [FromBody] UpdateProjectCommand cmd)
        {
            if (cmd.Id == Guid.Empty) cmd.Id = projectId;
            await _mediator.Send(cmd);
            return NoContent();
        }

        [HttpDelete("{projectId:guid}")]
        public async Task<IActionResult> Delete(Guid projectId)
        {
            await _mediator.Send(new DeleteProjectCommand { Id = projectId });
            return NoContent();
        }
    }
}