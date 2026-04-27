namespace AutismEdu.API.Models
{
    public class CommunicationCard : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string? Category { get; set; }
        #region     relationships
        public ICollection<ChildCard>? ChildCards { get; set; }
        #endregion
    }
}
