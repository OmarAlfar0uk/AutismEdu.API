using MediatR;

namespace AutismEdu.API.Features.Guidelines.Create
{
    public class CreateGuidelineCommand : IRequest<CreateGuidelineResponse>
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public IFormFile File { get; set; } = default!;
    }

    public class CreateGuidelineResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public long FileSizeKb { get; set; }
        public string DownloadUrl { get; set; } = default!;
    }
}
