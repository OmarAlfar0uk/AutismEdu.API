using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Activities.Get_All
{
    public class GetAllActivitiesHandler : IRequestHandler<GetAllActivitiesQuery, IEnumerable<ActivityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllActivitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ActivityDto>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Activity>();

            var list = await repo.GetAllAsync(false).ToListAsync(cancellationToken);

            return list.Select(a => new ActivityDto
            {
                Id = a.Id,
                Title = a.Title,
                PdfUrl = a.PdfUrl,
                Category = a.Category
            });
        }
    }
}
