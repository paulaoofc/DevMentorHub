using MediatR;
using DevMentorHub.Application.DTOs;

namespace DevMentorHub.Application.Commands
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class UpdateProjectCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class DeleteProjectCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}