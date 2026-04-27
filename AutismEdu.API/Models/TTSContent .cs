namespace AutismEdu.API.Models
{
    public class TTSContent : BaseEntity
    {
        public string Text { get; set; } = default!;
        public string? VoiceUrl { get; set; }
        public string? Lang { get; set; }
    }
}
