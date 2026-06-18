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
            var query = _uow.GetRepository<Models.CommunicationCard>()
                            .GetAllAsync();

            // ISSUE 1 FIX: Data isolation — specialists only see their own cards
            if (request.CurrentUserId.HasValue &&
                string.Equals(request.CurrentRole, "specialist", StringComparison.OrdinalIgnoreCase))
            {
                var userId = request.CurrentUserId.Value;
                query = query.Where(x => x.SpecialistId == userId || x.CreatedBy == userId);
            }

            var cards = await query.ToListAsync(cancellationToken);

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
