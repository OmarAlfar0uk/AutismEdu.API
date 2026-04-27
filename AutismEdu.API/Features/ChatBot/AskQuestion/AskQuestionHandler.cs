using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.ChatBot.AskQuestion
{
    public class AskQuestionHandler : IRequestHandler<AskQuestionCommand, AskQuestionResponse>
    {
        private readonly IGeminiService _geminiService;

        public AskQuestionHandler(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task<AskQuestionResponse> Handle(AskQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var answer = await _geminiService.AskAsync(request.Question);

                return new AskQuestionResponse
                {
                    Success = true,
                    Answer = answer
                };
            }
            catch (Exception ex)
            {
                return new AskQuestionResponse
                {
                    Success = false,
                    Answer = null,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
