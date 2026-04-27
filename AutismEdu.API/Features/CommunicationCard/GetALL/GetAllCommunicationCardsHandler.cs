using AutismEdu.API.Contracts;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.CommunicationCard.GetALL
{
    public class GetAllCommunicationCardsHandler : IRequestHandler<GetAllCommunicationCardsQuery, List<CommunicationCardDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;

        public GetAllCommunicationCardsHandler(IUnitOfWork uow, IImageHelper imageHelper)
        {
            _uow = uow;
            _imageHelper = imageHelper;
        }

        public async Task<List<CommunicationCardDto>> Handle(GetAllCommunicationCardsQuery request, CancellationToken cancellationToken)
        {
            var cards = await _uow.GetRepository<Models.CommunicationCard>()
                                  .GetAllAsync()
                                  .ToListAsync(cancellationToken);

            return cards.Select(card => new CommunicationCardDto
            {
                Id = card.Id,
                Title = card.Title,
                Category = card.Category,
                ImageUrl = _imageHelper.GetImageUrl(card.ImageUrl)
            }).ToList();
        }

    }
}
