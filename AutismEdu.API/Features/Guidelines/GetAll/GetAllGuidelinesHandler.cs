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

            // ISSUE 1 & 3 FIX: Guidelines isolation based on user role
            if (request.CurrentUserId.HasValue &&
                string.Equals(request.CurrentRole, "specialist", StringComparison.OrdinalIgnoreCase))
            {
                var userId = request.CurrentUserId.Value;
                query = query.Where(g => g.SpecialistId == userId || g.CreatedBy == userId);
            }
            else if (request.CurrentUserId.HasValue &&
                     string.Equals(request.CurrentRole, "parent", StringComparison.OrdinalIgnoreCase))
            {
                var parentId = request.CurrentUserId.Value;
                var childIds = await _uow.GetRepository<ChildProfile>()
                    .GetAllAsync()
                    .Where(c => c.UserId == parentId)
                    .Select(c => c.Id)
                    .ToListAsync(cancellationToken);

                var assignedGuidelineIds = await _uow.GetRepository<ChildGuideline>()
                    .GetAllAsync()
                    .Where(cg => childIds.Contains(cg.ChildId))
                    .Select(cg => cg.GuidelineId)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                query = query.Where(g => assignedGuidelineIds.Contains(g.Id));
            }
            else if (!request.CurrentUserId.HasValue || string.IsNullOrEmpty(request.CurrentRole))
            {
                query = query.Where(g => g.SpecialistId == null && g.CreatedBy == null);
            }

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
