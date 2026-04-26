using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Children.Update
{
    public class UpdateChildHandler : IRequestHandler<UpdateChildCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateChildHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateChildCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ChildProfile>();

            var child = await repo.GetByIdAsync(request.Id);
            if (child is null)
                throw new Exception("Child not found");

            child.Name = request.Name;
            child.Age = request.Age;
            child.Notes = request.Notes;

            repo.Update(child);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
