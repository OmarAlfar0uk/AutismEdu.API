using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.GetALL
{
    public class GetChildCardsHandler : IRequestHandler<GetChildCardsQuery, List<CommunicationCardDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;

        public GetChildCardsHandler(IUnitOfWork uow, IImageHelper imageHelper)
        {
            _uow = uow;
            _imageHelper = imageHelper;
        }

        public async Task<List<CommunicationCardDto>> Handle(GetChildCardsQuery request, CancellationToken cancellationToken)
        {
            var childCardsRepo = _uow.GetRepository<ChildCard>();

            var cardsRepo = _uow.GetRepository<Models.CommunicationCard>();

            var childCards = await childCardsRepo.FindAsync(x => x.ChildId == request.ChildId);

            var result = new List<CommunicationCardDto>();

            foreach (var cc in childCards)
            {
                var card = await cardsRepo.GetByIdAsync(cc.CardId);

                if (card != null)
                {
                    result.Add(new CommunicationCardDto
                    {
                        Id = card.Id,
                        Title = card.Title,
                        Category = card.Category,
                        ImageUrl = _imageHelper.GetImageUrl(card.ImageUrl)
                    });
                }
            }

            return result;
        }
    }
}
