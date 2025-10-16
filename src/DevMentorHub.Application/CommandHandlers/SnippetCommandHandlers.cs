using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevMentorHub.Application.Interfaces;
using DevMentorHub.Domain.Entities;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Application.Commands;
using System;

namespace DevMentorHub.Application.CommandHandlers
{
    public class CreateSnippetCommandHandler : IRequestHandler<CreateSnippetCommand, SnippetDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateSnippetCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<SnippetDto> Handle(CreateSnippetCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null || project.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException();

            var snippet = new Snippet
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                OwnerId = _currentUser.UserId,
                Language = request.Language,
                Code = request.Code,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Snippets.AddAsync(snippet, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new SnippetDto
            {
                Id = snippet.Id,
                ProjectId = snippet.ProjectId,
                Language = snippet.Language,
                Code = snippet.Code,
                Description = snippet.Description,
                CreatedAt = snippet.CreatedAt
            };
        }
    }
}