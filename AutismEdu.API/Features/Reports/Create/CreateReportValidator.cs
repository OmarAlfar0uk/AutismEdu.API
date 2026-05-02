using FluentValidation;

namespace AutismEdu.API.Features.Reports.Create
{
    public class CreateReportValidator : AbstractValidator<CreateReportCommand>
    {
        public CreateReportValidator()
        {
            RuleFor(x => x.IdStudent)
                .NotEmpty().WithMessage("Student ID is required.");

            RuleFor(x => x.Frequency)
                .IsInEnum().WithMessage("Invalid frequency value.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

            RuleFor(x => x.Skills)
                .NotEmpty().WithMessage("At least one skill is required.")
                .Must(s => s.All(skill => Enum.IsDefined(skill)))
                .WithMessage("Invalid skill type.");
        }
    }
}
