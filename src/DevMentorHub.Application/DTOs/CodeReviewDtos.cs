namespace DevMentorHub.Application.DTOs
{
    public class CodeReviewDto
    {
        public Guid Id { get; set; }
        public Guid SnippetId { get; set; }
        public string ResponseText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CodeReviewSummaryDto
    {
        public Guid Id { get; set; }
        public string ResponseTextSnippet { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}