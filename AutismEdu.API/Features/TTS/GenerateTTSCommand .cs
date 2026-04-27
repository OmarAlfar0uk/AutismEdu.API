using MediatR;

namespace AutismEdu.API.Features.TTS
{
    public class GenerateTTSCommand : IRequest<string>
    {
        public string Text { get; set; } = default!;
        public string Lang { get; set; } = "ar";
    }
}
