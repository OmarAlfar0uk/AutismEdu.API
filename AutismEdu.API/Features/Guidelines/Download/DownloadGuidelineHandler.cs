using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Guidelines.Download
{
    public class DownloadGuidelineHandler : IRequestHandler<DownloadGuidelineQuery, DownloadGuidelineResponse?>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _fileStorage;

        public DownloadGuidelineHandler(IUnitOfWork uow, IFileStorageService fileStorage)
        {
            _uow = uow;
            _fileStorage = fileStorage;
        }

        public async Task<DownloadGuidelineResponse?> Handle(DownloadGuidelineQuery request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Guideline>();
            var guideline = await repo.GetByIdAsync(request.Id);

            if (guideline == null)
                return null;

            var downloadUrl = _fileStorage.GetFileUrl(guideline.FilePath);
            var fileName = Path.GetFileName(guideline.FilePath);

            return new DownloadGuidelineResponse
            {
                DownloadUrl = downloadUrl,
                FileName = $"{guideline.Title}.pdf",
                FileSizeKb = guideline.FileSizeKb,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
