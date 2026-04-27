using FluentValidation;

namespace AutismEdu.API.Features.ChatBot.AskQuestion
{
    public class AskQuestionValidator : AbstractValidator<AskQuestionCommand>
    {
        public AskQuestionValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question is required")
                .MaximumLength(1000).WithMessage("Question must not exceed 1000 characters");
        }
    }
}
