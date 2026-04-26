using AutismEdu.API.Services;
using MediatR;

namespace AutismEdu.API.Features.TTS
{
    public class GenerateTTSHandler : IRequestHandler<GenerateTTSCommand, string>
    {
        private readonly TextToSpeechService _ttsService;

        public GenerateTTSHandler(TextToSpeechService ttsService)
        {
            _ttsService = ttsService;
        }

        public async Task<string> Handle(GenerateTTSCommand request, CancellationToken cancellationToken)
        {
            var url = _ttsService.GenerateAndSaveVoice(request.Text, request.Lang);
            return url;
        }
    }
}
