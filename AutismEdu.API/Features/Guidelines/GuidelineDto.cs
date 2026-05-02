namespace AutismEdu.API.Features.Guidelines
{
    public class GuidelineDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public long FileSizeKb { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
