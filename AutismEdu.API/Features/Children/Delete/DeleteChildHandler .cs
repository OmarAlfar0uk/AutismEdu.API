using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Children.Delete
{
    public class DeleteChildHandler : IRequestHandler<DeleteChildCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteChildHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteChildCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ChildProfile>();

            var child = await repo.GetByIdAsync(request.Id);
            if (child is null)
                throw new Exception("Child not found");

            repo.Delete(child);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
