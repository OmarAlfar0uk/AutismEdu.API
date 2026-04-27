using MediatR;

namespace AutismEdu.API.Features.ChatBot.AskQuestion
{
    public class AskQuestionCommand : IRequest<AskQuestionResponse>
    {
        public string Question { get; set; } = default!;
    }
}
