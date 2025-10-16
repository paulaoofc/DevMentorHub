using System;
using System.Collections.Generic;

namespace DevMentorHub.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public User Owner { get; set; }
        public ICollection<Snippet> Snippets { get; set; }
    }
}