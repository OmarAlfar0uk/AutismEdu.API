using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;


namespace AutismEdu.API.Features.Activities.Delete
{
    public class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteActivityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Activity>();
            var activity = await repo.GetByIdAsync(request.Id);

            if (activity is null)
                throw new Exception("Activity not found");

            repo.Delete(activity);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
