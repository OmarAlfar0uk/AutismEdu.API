namespace AutismEdu.API.Models
{
    public class Activity : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string PdfUrl { get; set; } = default!;
        public string? Category { get; set; }

        #region relationships
        public ICollection<ChildActivity>? ChildActivities { get; set; }

        #endregion
    }
}
