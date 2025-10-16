using FluentValidation;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.DTOs;

namespace DevMentorHub.Application.Validators
{
    public class CreateCodeReviewCommandValidator : AbstractValidator<CreateCodeReviewCommand>
    {
        public CreateCodeReviewCommandValidator()
        {
            RuleFor(x => x.SnippetId).NotEmpty();
            RuleFor(x => x.ReviewLevel).NotEmpty().Must(x => x == "quick" || x == "detailed");
        }
    }
}