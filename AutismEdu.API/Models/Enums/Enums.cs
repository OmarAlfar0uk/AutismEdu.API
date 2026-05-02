namespace AutismEdu.API.Models.Enums
{
    public enum ReportFrequency
    {
        Monthly,
        Quarterly,
        Annual
    }

    public enum ReportStatus
    {
        Draft,
        Published
    }

    public enum SkillType
    {
        SocialSkills,
        MotorSkills,
        Cognitive,
        Communication,
        SelfRegulation,
        Independence
    }

    public enum LessonLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }

    public enum PatientStatus
    {
        InProgress,
        Scheduled,
        NeedsReview
    }

    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }
}
