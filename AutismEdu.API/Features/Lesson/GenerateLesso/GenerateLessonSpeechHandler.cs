using AutismEdu.API.Contracts;
using AutismEdu.API.Data;
using AutismEdu.API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Lesson.GenerateLesso
{
    public class GenerateLessonSpeechHandler
        : IRequestHandler<GenerateLessonSpeechCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TextToSpeechService _tts;

        public GenerateLessonSpeechHandler(IUnitOfWork unitOfWork, TextToSpeechService tts)
        {
            _unitOfWork = unitOfWork;
            _tts = tts;
        }

        public async Task<string> Handle(GenerateLessonSpeechCommand request, CancellationToken cancellationToken)
        {
            var lessonRepo = _unitOfWork.GetRepository<Models.Lesson>();

            var lesson = await lessonRepo.GetByIdAsync(request.LessonId);

            if (lesson == null)
                throw new Exception("Lesson not found");

            var text = lesson.Title ?? lesson.Description ?? "no text provided";

            var url = _tts.GenerateAndSaveVoice(text, "ar");

            lesson.SpeechUrl = url;

            lessonRepo.Update(lesson);
            await _unitOfWork.SaveChangesAsync();

            return url;
        }
    }
}
