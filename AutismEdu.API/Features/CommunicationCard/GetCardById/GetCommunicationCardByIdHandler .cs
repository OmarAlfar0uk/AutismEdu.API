using AutismEdu.API.Contracts;
using AutismEdu.API.Shared;
using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.GetCardById
{
    public class GetCommunicationCardByIdHandler : IRequestHandler<GetCommunicationCardByIdQuery, CommunicationCardDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;

        public GetCommunicationCardByIdHandler(IUnitOfWork uow, IImageHelper helper)
        {
            _uow = uow;
            _imageHelper = helper;
        }

        public async Task<CommunicationCardDto?> Handle(GetCommunicationCardByIdQuery request, CancellationToken cancellationToken)
        {
            var card = await _uow.GetRepository<Models.CommunicationCard>().GetByIdAsync(request.Id);

            if (card == null) return null;

            return new CommunicationCardDto
            {
                Id = card.Id,
                Title = card.Title,
                Category = card.Category,
                ImageUrl = _imageHelper.GetImageUrl(card.ImageUrl)
            };
        }
    }
}
