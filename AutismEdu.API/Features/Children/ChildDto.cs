namespace AutismEdu.API.Features.Children
{
    public class ChildDto
    {
        public Guid Id { get; set; }
        public Guid ChildId { get; set; }
        public Guid PatientId { get; set; }
        public Guid UserId { get; set; }

        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? Notes { get; set; }
    }
}
