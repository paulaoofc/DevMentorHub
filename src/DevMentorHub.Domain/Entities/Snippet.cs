using System;
using System.Collections.Generic;

namespace DevMentorHub.Domain.Entities
{
    public class Snippet
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid OwnerId { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Project Project { get; set; }
        public User Owner { get; set; }
        public ICollection<CodeReview> CodeReviews { get; set; }
    }
}