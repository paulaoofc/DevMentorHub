namespace DevMentorHub.Application.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ProjectWithSnippetsDto : ProjectDto
    {
        public List<SnippetDto> Snippets { get; set; } = new();
    }
}