using FluentValidation;

namespace AutismEdu.API.Features.Guidelines.Create
{
    public class CreateGuidelineValidator : AbstractValidator<CreateGuidelineCommand>
    {
        private const long MaxFileSizeBytes = 20 * 1024 * 1024; // 20MB

        public CreateGuidelineValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(BeAPdfFile).WithMessage("Only PDF files are allowed.")
                .Must(BeWithinSizeLimit).WithMessage("File size must not exceed 20MB.");
        }

        private bool BeAPdfFile(IFormFile? file)
        {
            if (file == null) return false;
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            return extension == ".pdf" && file.ContentType == "application/pdf";
        }

        private bool BeWithinSizeLimit(IFormFile? file)
        {
            if (file == null) return false;
            return file.Length <= MaxFileSizeBytes;
        }
    }
}
