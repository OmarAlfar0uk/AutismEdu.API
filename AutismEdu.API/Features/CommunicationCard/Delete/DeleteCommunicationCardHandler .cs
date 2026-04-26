using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.Delete
{
    public class DeleteCommunicationCardHandler : IRequestHandler<DeleteCommunicationCardCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;

        public DeleteCommunicationCardHandler(IUnitOfWork uow, IImageHelper imageHelper)
        {
            _uow = uow;
            _imageHelper = imageHelper;
        }

        public async Task<bool> Handle(DeleteCommunicationCardCommand request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Models.CommunicationCard>();
            var card = await repo.GetByIdAsync(request.Id);

            if (card == null)
                return false;

            _imageHelper.DeleteImage(card.ImageUrl);

            repo.Delete(card);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
