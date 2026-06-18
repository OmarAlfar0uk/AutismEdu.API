namespace AutismEdu.API.Models
{
    public class Lesson : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string MediaUrl { get; set; } = default!;
        public string? SpeechUrl { get; set; }

        public string? Description { get; set; }
        public string? AudioUrl { get; set; }

        /// <summary>
        /// The specialist who created this lesson.
        /// </summary>
        public Guid? SpecialistId { get; set; }

        #region relationships
        public ICollection<PerformanceRecord>? PerformanceRecords { get; set; }
        #endregion
    }
}
