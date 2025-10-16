using AutoMapper;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Domain.Entities;

namespace DevMentorHub.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProjectCommand, Project>()
                .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Project, ProjectDto>();
            CreateMap<Project, ProjectWithSnippetsDto>();

            CreateMap<CreateSnippetCommand, Snippet>()
                .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Snippet, SnippetDto>();

            CreateMap<CodeReview, CodeReviewDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}