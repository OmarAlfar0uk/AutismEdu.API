using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Children.Add
{
    public class CreateChildHandler : IRequestHandler<CreateChildCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateChildHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateChildCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ChildProfile>();

            var child = new ChildProfile
            {
                UserId = request.UserId,
                Name = request.Name,
                Age = request.Age,
                Notes = request.Notes
            };

            await repo.CreateAsync(child);
            await _unitOfWork.SaveChangesAsync();

            return child.Id;
        }
    }
}
