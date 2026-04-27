namespace AutismEdu.API.Features.CommunicationCard
{
    public class CommunicationCardDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Category { get; set; }

        // Full URL (returned using ImageHelper in Handlers)
        public string ImageUrl { get; set; } = default!;
    }
}
