namespace AutismEdu.API.Features.Performance
{
    public class PerformanceDto
    {
        public Guid Id { get; set; }
        public Guid ChildId { get; set; }
        public Guid PatientId { get; set; }
        public Guid LessonId { get; set; }
        public int Score { get; set; }
        public int? TimeSpent { get; set; }
        public DateTime Date { get; set; }
    }
}
