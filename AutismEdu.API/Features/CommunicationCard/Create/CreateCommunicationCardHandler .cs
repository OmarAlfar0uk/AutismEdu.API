using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.Create
{
    public class CreateCommunicationCardHandler : IRequestHandler<CreateCommunicationCardCommand, Guid>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;

        public CreateCommunicationCardHandler(IUnitOfWork uow, IImageHelper imageHelper)
        {
            _uow = uow;
            _imageHelper = imageHelper;
        }

        public async Task<Guid> Handle(CreateCommunicationCardCommand request, CancellationToken cancellationToken)
        {
            // Save image using ImageHelper
            string relativePath = await _imageHelper.SaveImageAsync(
                request.Image,
                subFolder: "CommunicationCards"
            );

            // Create entity
            var card = new Models.CommunicationCard
            {
                Title = request.Title,
                Category = request.Category,
                ImageUrl = relativePath
            };

            await _uow.GetRepository<Models.CommunicationCard>().CreateAsync(card);
            await _uow.SaveChangesAsync();

            return card.Id;
        }
    }
}