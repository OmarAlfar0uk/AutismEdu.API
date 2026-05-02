using FluentValidation;

namespace AutismEdu.API.Features.Patients.Create
{
    public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0.");

            RuleFor(x => x.FocusArea)
                .NotEmpty().WithMessage("Focus area is required.")
                .MaximumLength(200).WithMessage("Focus area must not exceed 200 characters.");

            RuleFor(x => x.EmailParent)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.EmailParent))
                .WithMessage("Invalid email format.");
        }
    }
}
