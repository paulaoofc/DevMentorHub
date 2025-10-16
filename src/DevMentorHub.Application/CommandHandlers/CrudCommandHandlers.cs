using MediatR;
using DevMentorHub.Application.Interfaces;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Domain.Entities;

namespace DevMentorHub.Application.CommandHandlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public UpdateProjectCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _uow.Projects.GetByIdAsync(request.Id, cancellationToken);
            if (project == null || project.OwnerId != _currentUser.UserId) throw new UnauthorizedAccessException();
            project.Title = request.Title; project.Description = request.Description;
            _uow.Projects.Update(project);
            await _uow.CommitAsync(cancellationToken);
        }
    }

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public DeleteProjectCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _uow.Projects.GetByIdAsync(request.Id, cancellationToken);
            if (project == null || project.OwnerId != _currentUser.UserId) throw new UnauthorizedAccessException();
            _uow.Projects.Remove(project);
            await _uow.CommitAsync(cancellationToken);
        }
    }

    public class UpdateSnippetCommandHandler : IRequestHandler<UpdateSnippetCommand>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public UpdateSnippetCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task Handle(UpdateSnippetCommand request, CancellationToken cancellationToken)
        {
            var snippet = await _uow.Snippets.GetByIdAsync(request.Id, cancellationToken);
            if (snippet == null || snippet.OwnerId != _currentUser.UserId) throw new UnauthorizedAccessException();
            snippet.Language = request.Language; snippet.Code = request.Code; snippet.Description = request.Description;
            _uow.Snippets.Update(snippet);
            await _uow.CommitAsync(cancellationToken);
        }
    }

    public class DeleteSnippetCommandHandler : IRequestHandler<DeleteSnippetCommand>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser;
        public DeleteSnippetCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser) { _uow = uow; _currentUser = currentUser; }
        public async Task Handle(DeleteSnippetCommand request, CancellationToken cancellationToken)
        {
            var snippet = await _uow.Snippets.GetByIdAsync(request.Id, cancellationToken);
            if (snippet == null || snippet.OwnerId != _currentUser.UserId) throw new UnauthorizedAccessException();
            _uow.Snippets.Remove(snippet);
            await _uow.CommitAsync(cancellationToken);
        }
    }
}
