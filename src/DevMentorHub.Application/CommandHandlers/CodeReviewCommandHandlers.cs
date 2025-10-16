using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevMentorHub.Application.Interfaces;
using DevMentorHub.Domain.Entities;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Application.Commands;
using System;

namespace DevMentorHub.Application.CommandHandlers
{
    public class CreateCodeReviewCommandHandler : IRequestHandler<CreateCodeReviewCommand, CodeReviewDto>
    {
        private readonly IUnitOfWork _uow; private readonly ICurrentUserService _currentUser; private readonly IChatGptService _ai;
        public CreateCodeReviewCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser, IChatGptService ai) { _uow = uow; _currentUser = currentUser; _ai = ai; }
        public async Task<CodeReviewDto> Handle(CreateCodeReviewCommand request, CancellationToken cancellationToken)
        {
            var snippet = await _uow.Snippets.GetByIdAsync(request.SnippetId, cancellationToken);
            if (snippet == null || snippet.OwnerId != _currentUser.UserId) throw new UnauthorizedAccessException();

            var system = "Você é um code-reviewer experiente.";
            var user = $@"Analise o código abaixo ({snippet.Language}). Forneça:
  1) Principais problemas (bugs, segurança, performance)
  2) Sugestões de refatoração com exemplos de código
  3) Testes unitários recomendados
Código:
{snippet.Code}
Context: {snippet.Description}
Nível de detalhe: {request.ReviewLevel}";

            var responseText = await _ai.GenerateReviewAsync(system, user, cancellationToken);

            var review = new CodeReview
            {
                Id = Guid.NewGuid(),
                SnippetId = snippet.Id,
                RequesterId = _currentUser.UserId,
                Prompt = user,
                ResponseText = responseText,
                CreatedAt = DateTime.UtcNow,
                Status = CodeReviewStatus.Completed
            };
            await _uow.CodeReviews.AddAsync(review, cancellationToken);
            await _uow.CommitAsync(cancellationToken);

            return new CodeReviewDto { Id = review.Id, SnippetId = review.SnippetId, ResponseText = review.ResponseText, CreatedAt = review.CreatedAt, Status = review.Status.ToString() };
        }
    }
}