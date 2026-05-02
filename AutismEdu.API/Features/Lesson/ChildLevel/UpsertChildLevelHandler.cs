using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Lesson.ChildLevel
{
    public class UpsertChildLevelHandler : IRequestHandler<UpsertChildLevelCommand, ChildLevelDto>
    {
        private readonly IUnitOfWork _uow;

        public UpsertChildLevelHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ChildLevelDto> Handle(UpsertChildLevelCommand request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<ChildLessonLevel>();

            // Check if record already exists
            var existing = await repo.GetAllAsync(trackChanges: true)
                .Where(x => x.LessonId == request.LessonId && x.ChildId == request.IdChild)
                .FirstOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                // Update
                existing.Level = request.Level;
                existing.MasteryPercent = request.PercentMastery;
                existing.Notes = request.Notes;
                existing.LastUpdated = DateTime.UtcNow;
                repo.Update(existing);
            }
            else
            {
                // Create
                existing = new ChildLessonLevel
                {
                    LessonId = request.LessonId,
                    ChildId = request.IdChild,
                    Level = request.Level,
                    MasteryPercent = request.PercentMastery,
                    Notes = request.Notes,
                    LastUpdated = DateTime.UtcNow
                };
                await repo.CreateAsync(existing);
            }

            await _uow.SaveChangesAsync();

            return new ChildLevelDto
            {
                LessonId = existing.LessonId,
                ChildId = existing.ChildId,
                Level = existing.Level.ToString(),
                MasteryPercent = existing.MasteryPercent,
                Notes = existing.Notes,
                LastUpdated = existing.LastUpdated
            };
        }
    }
}
