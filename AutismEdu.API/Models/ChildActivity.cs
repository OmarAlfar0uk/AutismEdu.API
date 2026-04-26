namespace AutismEdu.API.Models
{
    public class ChildActivity : BaseEntity
    {
        public Guid ChildId { get; set; }
        public ChildProfile Child { get; set; }

        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }

        public DateTime? AssignedDate { get; set; }
    }
}
