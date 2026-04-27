namespace AutismEdu.API.Features.Activities
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string PdfUrl { get; set; } = default!;
        public string? Category { get; set; }
    }
}
