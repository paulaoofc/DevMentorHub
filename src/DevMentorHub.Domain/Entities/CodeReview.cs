using System;

namespace DevMentorHub.Domain.Entities
{
    public enum CodeReviewStatus
    {
        Pending,
        Completed
    }

    public class CodeReview
    {
        public Guid Id { get; set; }
        public Guid SnippetId { get; set; }
        public Guid RequesterId { get; set; }
        public string Prompt { get; set; }
        public string ResponseText { get; set; }
        public int? Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public CodeReviewStatus Status { get; set; }

        // Navigation
        public Snippet Snippet { get; set; }
        public User Requester { get; set; }
    }
}