namespace DevMentorHub.Application.DTOs
{
    public class SnippetDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}