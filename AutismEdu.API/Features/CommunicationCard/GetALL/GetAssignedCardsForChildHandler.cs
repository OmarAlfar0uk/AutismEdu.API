using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutismEdu.API.Features.CommunicationCard.GetALL
{
    public class GetAssignedCardsForChildHandler : IRequestHandler<GetAssignedCardsForChildQuery, List<AssignedCardDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;

        public GetAssignedCardsForChildHandler(IUnitOfWork uow, IImageHelper imageHelper)
        {
            _uow = uow;
            _imageHelper = imageHelper;
        }

        public async Task<List<AssignedCardDto>> Handle(GetAssignedCardsForChildQuery request, CancellationToken cancellationToken)
        {
            var childCardsRepo = _uow.GetRepository<ChildCard>();
            var cardsRepo = _uow.GetRepository<Models.CommunicationCard>();

            var childCards = await childCardsRepo.FindAsync(x => x.ChildId == request.ChildId);
            var result = new List<AssignedCardDto>();

            foreach (var cc in childCards)
            {
                var card = await cardsRepo.GetByIdAsync(cc.CardId);
                if (card != null)
                {
                    result.Add(new AssignedCardDto
                    {
                        Id = card.Id,
                        ImageName = _imageHelper.GetImageUrl(card.ImageUrl),
                        Category = card.Category,
                        AssignedAt = cc.CreatedAt
                    });
                }
            }

            return result;
        }
    }
}
