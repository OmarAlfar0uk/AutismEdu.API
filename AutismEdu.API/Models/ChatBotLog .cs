namespace AutismEdu.API.Models
{
    public class ChatBotLog : BaseEntity
    {
        public Guid ChildId { get; set; }
        public ChildProfile Child { get; set; }

        public string Question { get; set; } = default!;
        public string Answer { get; set; } = default!;

        public int? Attempts { get; set; }
        public DateTime? Date { get; set; }
    }
}
