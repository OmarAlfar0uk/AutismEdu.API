using AutismEdu.API.Models.Enums;

namespace AutismEdu.API.Models
{
    public class Appointment : BaseEntity
    {
        public Guid SpecialistId { get; set; }
        public ApplicationUser Specialist { get; set; } = default!;

        public Guid ChildId { get; set; }
        public ChildProfile Child { get; set; } = default!;

        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string? FocusArea { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    }
}
