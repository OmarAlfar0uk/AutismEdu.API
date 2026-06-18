using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AutismEdu.API.Features.Guidelines.Create
{
    public class CreateGuidelineHandler : IRequestHandler<CreateGuidelineCommand, CreateGuidelineResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _fileStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateGuidelineHandler(IUnitOfWork uow, IFileStorageService fileStorage, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _fileStorage = fileStorage;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateGuidelineResponse> Handle(CreateGuidelineCommand request, CancellationToken cancellationToken)
        {
            // Get current user ID from JWT
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
            Guid.TryParse(userIdStr, out var currentUserId);

            // Save file to storage
            var relativePath = await _fileStorage.SaveFileAsync(request.File, "Guidelines");
            var fileSizeKb = request.File.Length / 1024;

            // Create entity with ownership tracking
            var guideline = new Guideline
            {
                Title = request.Title,
                Description = request.Description,
                FilePath = relativePath,
                FileSizeKb = fileSizeKb,
                UploadedAt = DateTime.UtcNow,
                CreatedBy = currentUserId != Guid.Empty ? currentUserId : null,
                SpecialistId = currentUserId != Guid.Empty ? currentUserId : null
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
