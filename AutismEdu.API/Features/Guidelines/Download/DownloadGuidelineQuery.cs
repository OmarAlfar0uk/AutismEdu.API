using MediatR;

namespace AutismEdu.API.Features.Guidelines.Download
{
    public class DownloadGuidelineQuery : IRequest<DownloadGuidelineResponse?>
    {
        public Guid Id { get; set; }
    }

    public class DownloadGuidelineResponse
    {
        public string DownloadUrl { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public long FileSizeKb { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
