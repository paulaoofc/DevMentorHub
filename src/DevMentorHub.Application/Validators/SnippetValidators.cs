using FluentValidation;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.DTOs;

namespace DevMentorHub.Application.Validators
{
    public class CreateSnippetCommandValidator : AbstractValidator<CreateSnippetCommand>
    {
        public CreateSnippetCommandValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.Language).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }

    public class UpdateSnippetCommandValidator : AbstractValidator<UpdateSnippetCommand>
    {
        public UpdateSnippetCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Language).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}