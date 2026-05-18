namespace AutismEdu.API.Features.Reports
{
    public class ChildReportDto
    {
        public Guid Id { get; set; }
        public Guid ChildId { get; set; }
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public int LessonsCompleted { get; set; }
        public int ActivitiesCompleted { get; set; }
        public double AverageScore { get; set; }
        public int TotalTimeSpent { get; set; }
        public DateTime? LastActivityDate { get; set; }
    }
}
