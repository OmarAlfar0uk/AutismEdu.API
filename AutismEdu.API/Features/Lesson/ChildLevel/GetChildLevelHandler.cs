using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Lesson.ChildLevel
{
    public class GetChildLevelHandler : IRequestHandler<GetChildLevelQuery, ChildLevelDto?>
    {
        private readonly IUnitOfWork _uow;

        public GetChildLevelHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ChildLevelDto?> Handle(GetChildLevelQuery request, CancellationToken cancellationToken)
        {
            if (!request.ChildId.HasValue)
                return null;

            var repo = _uow.GetRepository<ChildLessonLevel>();
            var level = await repo.GetAllAsync()
                .Where(x => x.LessonId == request.LessonId && x.ChildId == request.ChildId.Value)
                .FirstOrDefaultAsync(cancellationToken);

            if (level == null)
                return null;

            return new ChildLevelDto
            {
                LessonId = level.LessonId,
                ChildId = level.ChildId,
                Level = level.Level.ToString(),
                MasteryPercent = level.MasteryPercent,
                Notes = level.Notes,
                LastUpdated = level.LastUpdated
            };
        }
    }
}
