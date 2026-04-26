using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.CommunicationCard.AssignCardToChild
{
    public class AssignCardToChildHandler : IRequestHandler<AssignCardToChildCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public AssignCardToChildHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(AssignCardToChildCommand request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<ChildCard>();

            // prevent duplicates
            var exists = await repo.FindAsync(x =>
                x.ChildId == request.ChildId && x.CardId == request.CardId
            );

            if (exists.Any())
                return true;

            var entity = new ChildCard
            {
                ChildId = request.ChildId,
                CardId = request.CardId,
                Enabled = true
            };

            await repo.CreateAsync(entity);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
