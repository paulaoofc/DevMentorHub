using MediatR;
using DevMentorHub.Application.DTOs;

namespace DevMentorHub.Application.Commands
{
    public class CreateSnippetCommand : IRequest<SnippetDto>
    {
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class UpdateSnippetCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class DeleteSnippetCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}