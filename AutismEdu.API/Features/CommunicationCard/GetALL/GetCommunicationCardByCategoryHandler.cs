using AutismEdu.API.Contracts;
using AutismEdu.API.Shared;
using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.GetALL
{
    public class GetCommunicationCardByCategoryHandler : IRequestHandler<GetCommunicationCardByCategoryQuery, List<CommunicationCardDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;

        public GetCommunicationCardByCategoryHandler(IUnitOfWork uow, IImageHelper imageHelper)
        {
            _uow = uow;
            _imageHelper = imageHelper;
        }

        public async Task<List<CommunicationCardDto>> Handle(GetCommunicationCardByCategoryQuery request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Models.CommunicationCard>();

            var cards = await repo.FindAsync(x => x.Category == request.Category);

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