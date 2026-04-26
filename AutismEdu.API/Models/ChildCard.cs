namespace AutismEdu.API.Models
{
    public class ChildCard:BaseEntity
    {
        public Guid ChildId { get; set; }
        public ChildProfile Child { get; set; }

        public Guid CardId { get; set; }
        public CommunicationCard Card { get; set; }

        public bool? Enabled { get; set; }
    }
}
