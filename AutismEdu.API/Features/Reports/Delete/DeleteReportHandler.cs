using AutismEdu.API.Contracts;
using AutismEdu.API.Exceptions;
using AutismEdu.API.Models;
using AutismEdu.API.Models.Enums;
using MediatR;

namespace AutismEdu.API.Features.Reports.Delete
{
    public class DeleteReportHandler : IRequestHandler<DeleteReportCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public DeleteReportHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Report>();
            var report = await repo.GetByIdAsync(request.Id);

            if (report == null)
                return false;

            // Business rule: only draft reports can be deleted
            if (report.Status == ReportStatus.Published)
                throw new ConflictException("Cannot delete a published report.");

            repo.Delete(report);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
