using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.AssignCardToChild
{
    public record AssignCardToChildCommand(Guid ChildId, Guid CardId) : IRequest<bool>;
}
