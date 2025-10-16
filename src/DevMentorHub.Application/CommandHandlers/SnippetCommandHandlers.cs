using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevMentorHub.Application.Interfaces;
using DevMentorHub.Domain.Entities;
using DevMentorHub.Application.DTOs;
using AutoMapper;
using DevMentorHub.Application.Commands;
using System;

namespace DevMentorHub.Application.CommandHandlers
{
    public class CreateSnippetCommandHandler : IRequestHandler<CreateSnippetCommand, SnippetDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateSnippetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<SnippetDto> Handle(CreateSnippetCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null || project.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException();

            var snippet = _mapper.Map<Snippet>(request);
            snippet.OwnerId = _currentUser.UserId;
            snippet.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.Snippets.AddAsync(snippet, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<SnippetDto>(snippet);
        }
    }
}