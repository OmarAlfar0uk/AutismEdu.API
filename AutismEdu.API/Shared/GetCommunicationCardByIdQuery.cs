using AutismEdu.API.Features.CommunicationCard;
using MediatR;

namespace AutismEdu.API.Shared
{
    

        public record GetCommunicationCardByIdQuery(Guid Id) : IRequest<CommunicationCardDto>;

}
