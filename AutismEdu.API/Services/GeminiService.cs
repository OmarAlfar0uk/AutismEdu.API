using AutismEdu.API.Contracts;
using System.Text;
using System.Text.Json;

namespace AutismEdu.API.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        private const string SystemPrompt = @"You are a warm and knowledgeable assistant specialized in autism spectrum disorder (ASD). 
You help parents, caregivers, and educators with guidance, tips, and support related to autism.
Respond naturally and conversationally — like a helpful friend who happens to be an expert.
Always respond in the same language the user writes in (Arabic or English).
Never add phrases like 'How can I support you today' or redirect the user at the end of every message.
Just answer naturally.";

        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> AskAsync(string question)
        {
            var apiKey = _config["GeminiSettings:ApiKey"];
            var model = _config["GeminiSettings:Model"] ?? "gemini-2.0-flash";

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var requestBody = new
            {
                system_instruction = new
                {
                    parts = new[] { new { text = SystemPrompt } }
                },
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = question } }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API returned {(int)response.StatusCode}: {errorBody}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);
            var answer = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return answer ?? "No response from Gemini.";
        }
    }
}
