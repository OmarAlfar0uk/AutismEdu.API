using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Guidelines.Delete
{
    public class DeleteGuidelineHandler : IRequestHandler<DeleteGuidelineCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public DeleteGuidelineHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteGuidelineCommand request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Guideline>();
            var guideline = await repo.GetByIdAsync(request.Id);

            if (guideline == null)
                return false;

            // Soft delete via GenericRepository.Delete()
            repo.Delete(guideline);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
