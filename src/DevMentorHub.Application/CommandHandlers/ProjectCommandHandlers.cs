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
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Project>(request);
            project.OwnerId = _currentUser.UserId;
            project.CreatedAt = DateTime.UtcNow;
            _unitOfWork.Projects.Add(project);
            await _unitOfWork.CommitAsync(cancellationToken);
            return _mapper.Map<ProjectDto>(project);
        }
    }
}