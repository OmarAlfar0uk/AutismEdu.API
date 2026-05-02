namespace AutismEdu.API.Features.Dashboard.Overview
{
    public class OverviewDto
    {
        public int ActivePatients { get; set; }
        public int SessionsToday { get; set; }
        public int ReviewsPending { get; set; }
        public float WeeklyImpactPercent { get; set; }
        public List<TodayAppointmentDto> TodaysSchedule { get; set; } = new();
    }

    public class TodayAppointmentDto
    {
        public string TimeStart { get; set; } = default!;
        public string TimeEnd { get; set; } = default!;
        public string PatientName { get; set; } = default!;
        public string? FocusArea { get; set; }
        public string Status { get; set; } = default!;
    }
}
