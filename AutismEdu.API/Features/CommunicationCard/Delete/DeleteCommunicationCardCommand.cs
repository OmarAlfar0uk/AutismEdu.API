using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.Delete
{
    public record DeleteCommunicationCardCommand(Guid Id) : IRequest<bool>;
}
