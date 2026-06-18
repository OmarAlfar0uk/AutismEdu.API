using System;

namespace AutismEdu.API.Features.CommunicationCard
{
    public class AssignedCardDto
    {
        public Guid Id { get; set; }
        public string ImageName { get; set; } = default!;
        public string? Category { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
