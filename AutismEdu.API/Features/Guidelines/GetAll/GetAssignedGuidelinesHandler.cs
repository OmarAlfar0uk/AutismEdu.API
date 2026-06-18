using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutismEdu.API.Features.Guidelines.GetAll
{
    public class GetAssignedGuidelinesHandler : IRequestHandler<GetAssignedGuidelinesQuery, List<AssignedGuidelineDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetAssignedGuidelinesHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<AssignedGuidelineDto>> Handle(GetAssignedGuidelinesQuery request, CancellationToken cancellationToken)
        {
            var childGuidelinesRepo = _uow.GetRepository<ChildGuideline>();
            var guidelinesRepo = _uow.GetRepository<Models.Guideline>();

            var childGuidelines = await childGuidelinesRepo.FindAsync(x => x.ChildId == request.ChildId);
            var result = new List<AssignedGuidelineDto>();

            foreach (var cg in childGuidelines)
            {
                var guideline = await guidelinesRepo.GetByIdAsync(cg.GuidelineId);
                if (guideline != null)
                {
                    result.Add(new AssignedGuidelineDto
                    {
                        Id = guideline.Id,
                        Title = guideline.Title,
                        Description = guideline.Description,
                        FileSizeKb = guideline.FileSizeKb,
                        DownloadUrl = $"{request.BaseUrl.TrimEnd('/')}/api/Guidelines/{guideline.Id}/download",
                        AssignedAt = cg.AssignedAt
                    });
                }
            }

            return result;
        }
    }
}
