using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Guidelines.GetAll
{
    public class GetAllGuidelinesHandler : IRequestHandler<GetAllGuidelinesQuery, GetAllGuidelinesResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetAllGuidelinesHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GetAllGuidelinesResponse> Handle(GetAllGuidelinesQuery request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Guideline>();
            var query = repo.GetAllAsync();

            var total = await query.CountAsync(cancellationToken);

            var guidelines = await query
                .OrderByDescending(g => g.UploadedAt)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .Select(g => new GuidelineDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    FileSizeKb = g.FileSizeKb,
                    UploadedAt = g.UploadedAt
                })
                .ToListAsync(cancellationToken);

            return new GetAllGuidelinesResponse
            {
                Guidelines = guidelines,
                Total = total,
                Page = request.Page,
                TotalPages = (int)Math.Ceiling(total / (double)request.Limit)
            };
        }
    }
}
