using DevMentorHub.Application.Interfaces;
using DevMentorHub.Domain.Entities;
using DevMentorHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DevMentorHub.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DevMentorHubDbContext _db;
        public UserRepository(DevMentorHubDbContext db) { _db = db; }
        public Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default) => _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);
        public async Task AddUserAsync(User user, CancellationToken ct = default)
        {
            await _db.Users.AddAsync(user, ct);
        }
    }

    public class ProjectRepository : IProjectRepository
    {
        private readonly DevMentorHubDbContext _db;
        public ProjectRepository(DevMentorHubDbContext db) { _db = db; }
        public void Add(Project project) => _db.Projects.Add(project);
        public Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default) => _db.Projects.Include(p => p.Snippets).FirstOrDefaultAsync(p => p.Id == id, ct);
        public Task<List<Project>> GetByOwnerAsync(Guid ownerId, CancellationToken ct = default) => _db.Projects.Where(p => p.OwnerId == ownerId).ToListAsync(ct);
        public void Update(Project project) => _db.Projects.Update(project);
        public void Remove(Project project) => _db.Projects.Remove(project);
    }

    public class SnippetRepository : ISnippetRepository
    {
        private readonly DevMentorHubDbContext _db;
        public SnippetRepository(DevMentorHubDbContext db) { _db = db; }
        public async Task AddAsync(Snippet snippet, CancellationToken ct = default) => await _db.Snippets.AddAsync(snippet, ct);
        public Task<Snippet?> GetByIdAsync(Guid id, CancellationToken ct = default) => _db.Snippets.Include(s => s.CodeReviews).FirstOrDefaultAsync(s => s.Id == id, ct);
        public void Update(Snippet snippet) => _db.Snippets.Update(snippet);
        public void Remove(Snippet snippet) => _db.Snippets.Remove(snippet);
        public Task<List<Snippet>> GetByProjectAsync(Guid projectId, CancellationToken ct = default) => _db.Snippets.Where(s => s.ProjectId == projectId).ToListAsync(ct);
    }

    public class CodeReviewRepository : ICodeReviewRepository
    {
        private readonly DevMentorHubDbContext _db;
        public CodeReviewRepository(DevMentorHubDbContext db) { _db = db; }
        public async Task AddAsync(CodeReview review, CancellationToken ct = default) => await _db.CodeReviews.AddAsync(review, ct);
        public Task<List<CodeReview>> GetBySnippetAsync(Guid snippetId, CancellationToken ct = default) => _db.CodeReviews.Where(r => r.SnippetId == snippetId).ToListAsync(ct);
        public Task<CodeReview?> GetByIdAsync(Guid id, CancellationToken ct = default) => _db.CodeReviews.FirstOrDefaultAsync(r => r.Id == id, ct);
    }
}
