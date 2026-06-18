using AutismEdu.API.Features.CommunicationCard;
using MediatR;

namespace AutismEdu.API.Shared
{
    /// <summary>
    /// Query to get cards assigned to a child, with parent ownership validation.
    /// Returns null if the child does not belong to the given parent.
    /// </summary>
    public record GetChildCardsWithParentValidationQuery(Guid ChildId, Guid ParentUserId) : IRequest<List<CommunicationCardDto>?>;
}
