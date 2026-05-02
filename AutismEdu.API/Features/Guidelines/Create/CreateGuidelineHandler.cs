using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Guidelines.Create
{
    public class CreateGuidelineHandler : IRequestHandler<CreateGuidelineCommand, CreateGuidelineResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _fileStorage;

        public CreateGuidelineHandler(IUnitOfWork uow, IFileStorageService fileStorage)
        {
            _uow = uow;
            _fileStorage = fileStorage;
        }

        public async Task<CreateGuidelineResponse> Handle(CreateGuidelineCommand request, CancellationToken cancellationToken)
        {
            // Save file to storage
            var relativePath = await _fileStorage.SaveFileAsync(request.File, "Guidelines");
            var fileSizeKb = request.File.Length / 1024;

            // Create entity
            var guideline = new Guideline
            {
                Title = request.Title,
                Description = request.Description,
                FilePath = relativePath,
                FileSizeKb = fileSizeKb,
                UploadedAt = DateTime.UtcNow
            };

            var repo = _uow.GetRepository<Guideline>();
            await repo.CreateAsync(guideline);
            await _uow.SaveChangesAsync();

            return new CreateGuidelineResponse
            {
                Id = guideline.Id,
                Title = guideline.Title,
                Description = guideline.Description,
                FileSizeKb = guideline.FileSizeKb,
                DownloadUrl = _fileStorage.GetFileUrl(relativePath)
            };
        }
    }
}
