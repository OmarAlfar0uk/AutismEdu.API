using FluentValidation;

namespace AutismEdu.API.Features.Lesson.ChildLevel
{
    public class UpsertChildLevelValidator : AbstractValidator<UpsertChildLevelCommand>
    {
        public UpsertChildLevelValidator()
        {
            RuleFor(x => x.IdChild)
                .NotEmpty().WithMessage("Child ID is required.");

            RuleFor(x => x.Level)
                .IsInEnum().WithMessage("Invalid level value.");

            RuleFor(x => x.PercentMastery)
                .InclusiveBetween(0, 100).WithMessage("Mastery percent must be between 0 and 100.");
        }
    }
}
