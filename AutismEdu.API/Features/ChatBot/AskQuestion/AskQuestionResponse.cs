namespace AutismEdu.API.Features.ChatBot.AskQuestion
{
    public class AskQuestionResponse
    {
        public bool Success { get; set; }
        public string? Answer { get; set; }
        public Guid? LogId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
