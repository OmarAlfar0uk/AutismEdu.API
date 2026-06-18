using AutismEdu.API.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AutismEdu.API.Features.Lesson.Add
{
    public class CreateLessonHandler : IRequestHandler<CreateLessonCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateLessonHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            // Get current user ID from JWT
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
            Guid.TryParse(userIdStr, out var currentUserId);

            var repo = _unitOfWork.GetRepository<Models.Lesson>();

            var lesson = new Models.Lesson
            {
                Title = request.Title,
                Type = request.Type,
                MediaUrl = request.MediaUrl,
                Description = request.Description,
                AudioUrl = request.AudioUrl,
                CreatedBy = currentUserId != Guid.Empty ? currentUserId : null,
                SpecialistId = currentUserId != Guid.Empty ? currentUserId : null
            };

            await repo.CreateAsync(lesson);
            await _unitOfWork.SaveChangesAsync();

            return lesson.Id;
        }
    }
}
