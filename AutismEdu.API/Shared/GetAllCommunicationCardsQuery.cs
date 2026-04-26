using AutismEdu.API.Features.CommunicationCard;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetAllCommunicationCardsQuery() : IRequest<List<CommunicationCardDto>>;
}
