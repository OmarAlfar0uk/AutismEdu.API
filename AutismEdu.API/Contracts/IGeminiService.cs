namespace AutismEdu.API.Contracts
{
    public interface IGeminiService
    {
        Task<string> AskAsync(string question);
    }
}
