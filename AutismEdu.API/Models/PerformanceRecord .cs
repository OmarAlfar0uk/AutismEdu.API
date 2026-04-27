namespace AutismEdu.API.Models
{
    public class PerformanceRecord : BaseEntity
    {
        public Guid ChildId { get; set; }
        public ChildProfile Child { get; set; }

        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public int Attempts { get; set; }

        public int Score { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int? TimeSpent { get; set; }
    }
}
