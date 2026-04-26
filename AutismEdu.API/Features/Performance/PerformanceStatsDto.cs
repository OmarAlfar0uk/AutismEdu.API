namespace AutismEdu.API.Features.Performance
{
    public class PerformanceStatsDto
    {
        public int TotalLessonsCompleted { get; set; }
        public double AverageScore { get; set; }
        public int TotalTimeSpent { get; set; }
        public string? BestLesson { get; set; }
        public string? WeakestLesson { get; set; }
    }
}
