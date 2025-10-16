using System.Threading;
using System.Threading.Tasks;

namespace DevMentorHub.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<Domain.Entities.User?> GetUserByEmailAsync(string email, CancellationToken ct = default);
        Task AddUserAsync(Domain.Entities.User user, CancellationToken ct = default);
    }

    public interface IProjectRepository
    {
        void Add(Domain.Entities.Project project);
        Task<Domain.Entities.Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Domain.Entities.Project>> GetByOwnerAsync(Guid ownerId, CancellationToken ct = default);
        void Update(Domain.Entities.Project project);
        void Remove(Domain.Entities.Project project);
    }

    public interface ISnippetRepository
    {
        Task AddAsync(Domain.Entities.Snippet snippet, CancellationToken ct = default);
        Task<Domain.Entities.Snippet?> GetByIdAsync(Guid id, CancellationToken ct = default);
        void Update(Domain.Entities.Snippet snippet);
        void Remove(Domain.Entities.Snippet snippet);
        Task<List<Domain.Entities.Snippet>> GetByProjectAsync(Guid projectId, CancellationToken ct = default);
    }

    public interface ICodeReviewRepository
    {
        Task AddAsync(Domain.Entities.CodeReview review, CancellationToken ct = default);
        Task<List<Domain.Entities.CodeReview>> GetBySnippetAsync(Guid snippetId, CancellationToken ct = default);
        Task<Domain.Entities.CodeReview?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }

    public interface IUnitOfWork
    {
        IProjectRepository Projects { get; }
        ISnippetRepository Snippets { get; }
        ICodeReviewRepository CodeReviews { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
    }

    public interface IAuthService
    {
        string GenerateJwtToken(Domain.Entities.User user);
    }

    public interface IChatGptService
    {
        Task<string> GenerateReviewAsync(string systemPrompt, string userPrompt, CancellationToken ct = default);
    }
}