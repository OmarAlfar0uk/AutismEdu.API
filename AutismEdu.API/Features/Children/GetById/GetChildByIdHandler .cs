using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;

namespace AutismEdu.API.Features.Children.GetById
{
    public class GetChildByIdHandler : IRequestHandler<GetChildByIdQuery, ChildDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChildByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ChildDto?> Handle(GetChildByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ChildProfile>();

            var child = await repo.GetByIdAsync(request.Id);
            if (child is null)
                return null;

            return new ChildDto
            {
                Id = child.Id,
                ChildId = child.Id,
                PatientId = child.Id,
                UserId = child.UserId,
                Name = child.Name,
                Age = child.Age,
                Notes = child.Notes
            };
        }
    }
}
