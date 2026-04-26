using AutismEdu.API.Features.CommunicationCard;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetCommunicationCardByCategoryQuery(string Category)
        : IRequest<List<CommunicationCardDto>>;
}
