using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Children.GetById
{
    public class GetChildrenByParentHandler
          : IRequestHandler<GetChildrenByParentQuery, IEnumerable<ChildDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChildrenByParentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ChildDto>> Handle(GetChildrenByParentQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ChildProfile>();

            var query = repo
                .GetAllAsync(trackChanges: false)
                .Where(c => c.UserId == request.UserId);

            var list = await query.ToListAsync(cancellationToken);

            return list.Select(c => new ChildDto
            {
                Id = c.Id,
                ChildId = c.Id,
                PatientId = c.Id,
                UserId = c.UserId,
                Name = c.Name,
                Age = c.Age,
                Notes = c.Notes
            });
        }
    }
}
