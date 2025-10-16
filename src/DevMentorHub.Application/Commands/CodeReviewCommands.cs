using MediatR;
using DevMentorHub.Application.DTOs;

namespace DevMentorHub.Application.Commands
{
    public class CreateCodeReviewCommand : IRequest<CodeReviewDto>
    {
        public Guid SnippetId { get; set; }
        public string RequestComment { get; set; }
        public string ReviewLevel { get; set; }
    }
}