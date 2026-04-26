using MediatR;

namespace AutismEdu.API.Features.Activities.Create
{
    public class CreateActivityCommand : IRequest<Guid>
    {
        public string Title { get; set; } = default!;
        public string? Category { get; set; }
        public IFormFile PdfFile { get; set; } = default!;
    }
}
