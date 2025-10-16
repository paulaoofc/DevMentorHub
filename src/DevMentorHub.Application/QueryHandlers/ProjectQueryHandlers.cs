using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevMentorHub.Application.Interfaces;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Application.Queries;
using System.Collections.Generic;
using System.Linq;

namespace DevMentorHub.Application.QueryHandlers
{
    public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;
        public GetUserProjectsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
        {
            _uow = uow; _currentUser = currentUser;
        }
        public async Task<IEnumerable<ProjectDto>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var list = await _uow.Projects.GetByOwnerAsync(_currentUser.UserId, cancellationToken);
            return list.Select(p => new ProjectDto { Id = p.Id, Title = p.Title, Description = p.Description, CreatedAt = p.CreatedAt });
        }
    }

    public class GetProjectWithSnippetsQueryHandler : IRequestHandler<GetProjectWithSnippetsQuery, ProjectWithSnippetsDto>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public GetProjectWithSnippetsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task<ProjectWithSnippetsDto?> Handle(GetProjectWithSnippetsQuery request, CancellationToken cancellationToken)
        {
            var project = await _uow.Projects.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null || project.OwnerId != _currentUser.UserId) return null;
            return new ProjectWithSnippetsDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                Snippets = project.Snippets?.Select(s => new SnippetDto { Id = s.Id, ProjectId = s.ProjectId, Language = s.Language, Code = s.Code, Description = s.Description, CreatedAt = s.CreatedAt }).ToList() ?? new()
            };
        }
    }

    public class GetSnippetQueryHandler : IRequestHandler<GetSnippetQuery, SnippetDto>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public GetSnippetQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task<SnippetDto?> Handle(GetSnippetQuery request, CancellationToken cancellationToken)
        {
            var s = await _uow.Snippets.GetByIdAsync(request.SnippetId, cancellationToken);
            if (s == null || s.OwnerId != _currentUser.UserId) return null;
            return new SnippetDto { Id = s.Id, ProjectId = s.ProjectId, Language = s.Language, Code = s.Code, Description = s.Description, CreatedAt = s.CreatedAt };
        }
    }

    public class GetSnippetReviewsQueryHandler : IRequestHandler<GetSnippetReviewsQuery, IEnumerable<CodeReviewSummaryDto>>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public GetSnippetReviewsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task<IEnumerable<CodeReviewSummaryDto>> Handle(GetSnippetReviewsQuery request, CancellationToken cancellationToken)
        {
            var snippet = await _uow.Snippets.GetByIdAsync(request.SnippetId, cancellationToken);
            if (snippet == null || snippet.OwnerId != _currentUser.UserId) return [];
            var list = await _uow.CodeReviews.GetBySnippetAsync(request.SnippetId, cancellationToken);
            return list.Select(r => new CodeReviewSummaryDto { Id = r.Id, ResponseTextSnippet = r.ResponseText?.Split('\n')?[0] ?? string.Empty, CreatedAt = r.CreatedAt });
        }
    }

    public class GetCodeReviewByIdQueryHandler : IRequestHandler<GetCodeReviewByIdQuery, CodeReviewDto>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public GetCodeReviewByIdQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task<CodeReviewDto?> Handle(GetCodeReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _uow.CodeReviews.GetByIdAsync(request.ReviewId, cancellationToken);
            if (review == null) return null;
            var snippet = await _uow.Snippets.GetByIdAsync(review.SnippetId, cancellationToken);
            if (snippet == null || snippet.OwnerId != _currentUser.UserId) return null;
            return new CodeReviewDto { Id = review.Id, SnippetId = review.SnippetId, ResponseText = review.ResponseText, CreatedAt = review.CreatedAt, Status = review.Status.ToString() };
        }
    }
}