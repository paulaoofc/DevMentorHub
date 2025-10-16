using MediatR;
using DevMentorHub.Application.DTOs;
using System.Collections.Generic;

namespace DevMentorHub.Application.Queries
{
    public class GetUserProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
    }

    public class GetProjectWithSnippetsQuery : IRequest<ProjectWithSnippetsDto>
    {
        public Guid ProjectId { get; set; }
    }

    public class GetSnippetQuery : IRequest<SnippetDto>
    {
        public Guid SnippetId { get; set; }
    }

    public class GetSnippetReviewsQuery : IRequest<IEnumerable<CodeReviewSummaryDto>>
    {
        public Guid SnippetId { get; set; }
    }

    public class GetCodeReviewByIdQuery : IRequest<CodeReviewDto>
    {
        public Guid ReviewId { get; set; }
    }
}