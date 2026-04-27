using AutismEdu.API.Data;
using AutismEdu.API.Models;
using System.Speech.Synthesis;

namespace AutismEdu.API.Services
{
    public class TextToSpeechService
    {
        private readonly ApplicationDbContext _context;

        public TextToSpeechService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GenerateAndSaveVoice(string text, string lang = "ar")
        {
            var synth = new SpeechSynthesizer();

            var fileName = $"tts_{Guid.NewGuid()}.wav";
            var folderPath = Path.Combine("wwwroot", "tts");
            var filePath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            synth.SetOutputToWaveFile(filePath);
            synth.Speak(text);

            var entity = new TTSContent
            {
                Text = text,
                VoiceUrl = $"/tts/{fileName}",
                Lang = lang
            };

            _context.TTSContents.Add(entity);
            _context.SaveChanges();

            return entity.VoiceUrl!;
        }
    }
}
