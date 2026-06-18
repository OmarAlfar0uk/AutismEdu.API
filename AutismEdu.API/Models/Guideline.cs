namespace AutismEdu.API.Models
{
    public class Guideline : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string FilePath { get; set; } = default!;
        public long FileSizeKb { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The specialist who uploaded this guideline.
        /// </summary>
        public Guid? SpecialistId { get; set; }
    }
}
