using AutismEdu.API.Features.CommunicationCard;
using MediatR;

namespace AutismEdu.API.Shared
{
    public class GetAllCommunicationCardsQuery : IRequest<List<CommunicationCardDto>>
    {
        public Guid? CurrentUserId { get; set; }

        public string? CurrentRole { get; set; }
    }
}
