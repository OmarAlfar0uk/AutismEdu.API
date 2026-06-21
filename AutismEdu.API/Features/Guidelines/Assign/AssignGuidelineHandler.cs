using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Guidelines.Assign
{
    public class AssignGuidelineHandler : IRequestHandler<AssignGuidelineCommand, AssignGuidelineResult>
    {
        private readonly IUnitOfWork _uow;

        public AssignGuidelineHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<AssignGuidelineResult> Handle(AssignGuidelineCommand request, CancellationToken cancellationToken)
        {
            var guidelineRepo = _uow.GetRepository<Models.Guideline>();
            var guideline = await guidelineRepo.GetByIdAsync(request.GuidelineId);
            if (guideline == null)
            {
                return new AssignGuidelineResult
                {
                    Success = false,
                    StatusCode = 404,
                    ErrorMessage = "Guideline not found"
                };
            }

            var childRepo = _uow.GetRepository<ChildProfile>();
            var child = await childRepo.GetByIdAsync(request.ChildId);
            if (child == null)
            {
                return new AssignGuidelineResult
                {
                    Success = false,
                    StatusCode = 404,
                    ErrorMessage = "Child not found"
                };
            }

            var childGuidelineRepo = _uow.GetRepository<ChildGuideline>();
            var existing = await childGuidelineRepo.FindAsync(cg => cg.ChildId == request.ChildId && cg.GuidelineId == request.GuidelineId);
            if (existing.Any())
            {
                return new AssignGuidelineResult
                {
                    Success = false,
                    StatusCode = 409,
                    ErrorMessage = "Guideline is already assigned to this child"
                };
            }

            var childGuideline = new ChildGuideline
            {
                GuidelineId = request.GuidelineId,
                ChildId = request.ChildId,
                AssignedBy = request.AssignedBy,
                AssignedAt = DateTime.UtcNow
            };

            await childGuidelineRepo.CreateAsync(childGuideline);
            await _uow.SaveChangesAsync();

            return new AssignGuidelineResult
            {
                Success = true
            };
        }
    }
}
