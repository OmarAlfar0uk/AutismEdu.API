using System;
using System.Collections.Generic;
using AutismEdu.API.Features.CommunicationCard;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetAssignedCardsForChildQuery(Guid ChildId) : IRequest<List<AssignedCardDto>>;
}
