using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevMentorHub.Application.Interfaces;
using DevMentorHub.Application.DTOs;
using AutoMapper;
using DevMentorHub.Application.Queries;
using System.Collections.Generic;

namespace DevMentorHub.Application.QueryHandlers
{
    public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        public GetUserProjectsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
        {
            _uow = uow; _currentUser = currentUser; _mapper = mapper;
        }
        public async Task<IEnumerable<ProjectDto>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var list = await _uow.Projects.GetByOwnerAsync(_currentUser.UserId, cancellationToken);
            return _mapper.Map<IEnumerable<ProjectDto>>(list);
        }
    }

    public class GetProjectWithSnippetsQueryHandler : IRequestHandler<GetProjectWithSnippetsQuery, ProjectWithSnippetsDto>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser; private readonly IMapper _mapper;
        public GetProjectWithSnippetsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper) { _uow = uow; _currentUser = currentUser; _mapper = mapper; }
        public async Task<ProjectWithSnippetsDto?> Handle(GetProjectWithSnippetsQuery request, CancellationToken cancellationToken)
        {
            var project = await _uow.Projects.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null || project.OwnerId != _currentUser.UserId) return null;
            return _mapper.Map<ProjectWithSnippetsDto>(project);
        }
    }

    public class GetSnippetQueryHandler : IRequestHandler<GetSnippetQuery, SnippetDto>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser; private readonly IMapper _mapper;
        public GetSnippetQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper) { _uow = uow; _currentUser = currentUser; _mapper = mapper; }
        public async Task<SnippetDto?> Handle(GetSnippetQuery request, CancellationToken cancellationToken)
        {
            var snippet = await _uow.Snippets.GetByIdAsync(request.SnippetId, cancellationToken);
            if (snippet == null || snippet.OwnerId != _currentUser.UserId) return null;
            return _mapper.Map<SnippetDto>(snippet);
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
            return list.ConvertAll(r => new CodeReviewSummaryDto { Id = r.Id, ResponseTextSnippet = r.ResponseText?.Split('\n')?[0] ?? string.Empty, CreatedAt = r.CreatedAt });
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