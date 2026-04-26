using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.Create
{
    public record CreateCommunicationCardCommand(
        IFormFile Image,
        string Title,
        string? Category
    ) : IRequest<Guid>;
}
