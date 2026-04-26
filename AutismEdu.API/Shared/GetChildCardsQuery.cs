using AutismEdu.API.Features.CommunicationCard;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetChildCardsQuery(Guid ChildId) : IRequest<List<CommunicationCardDto>>;
}
