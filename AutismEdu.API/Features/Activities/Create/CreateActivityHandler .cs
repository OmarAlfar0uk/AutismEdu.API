using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Activities.Create
{
    public class CreateActivityHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public CreateActivityHandler(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public async Task<Guid> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var folder = Path.Combine(_env.WebRootPath, "activities");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}_{request.PdfFile.FileName}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.PdfFile.CopyToAsync(stream, cancellationToken);
            }

            var activity = new Activity
            {
                Title = request.Title,
                Category = request.Category,
                PdfUrl = $"/activities/{fileName}"
            };

            var repo = _unitOfWork.GetRepository<Activity>();
            await repo.CreateAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            return activity.Id;
        }
    }
}
