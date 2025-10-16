using DevMentorHub.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DevMentorHub.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DevMentorHubDbContext _db;
        public UnitOfWork(DevMentorHubDbContext db, IProjectRepository projects, ISnippetRepository snippets, ICodeReviewRepository codeReviews)
        {
            _db = db;
            Projects = projects;
            Snippets = snippets;
            CodeReviews = codeReviews;
        }

        public IProjectRepository Projects { get; }
        public ISnippetRepository Snippets { get; }
        public ICodeReviewRepository CodeReviews { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _db.SaveChangesAsync(cancellationToken);
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}