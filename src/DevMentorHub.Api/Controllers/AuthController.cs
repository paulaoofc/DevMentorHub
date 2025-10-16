using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Application.Queries;

namespace DevMentorHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) { _mediator = mediator; }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterUserCommand cmd)
        {
            var res = await _mediator.Send(cmd);
            if (res is null) return Conflict();
            return Ok(res);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserCommand cmd)
        {
            var res = await _mediator.Send(cmd);
            if (res is null) return Unauthorized();
            return Ok(res);
        }
    }
}