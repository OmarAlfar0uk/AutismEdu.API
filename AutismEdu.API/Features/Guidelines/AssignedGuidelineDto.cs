using System;

namespace AutismEdu.API.Features.Guidelines
{
    public class AssignedGuidelineDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public long FileSizeKb { get; set; }
        public string DownloadUrl { get; set; } = default!;
        public DateTime AssignedAt { get; set; }
    }
}
