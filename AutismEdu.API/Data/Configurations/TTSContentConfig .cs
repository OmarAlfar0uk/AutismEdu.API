using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class TTSContentConfig : IEntityTypeConfiguration<TTSContent>
    {
        public void Configure(EntityTypeBuilder<TTSContent> builder)
        {
            builder.Property(t => t.Text)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.VoiceUrl)
                .HasMaxLength(500);

            builder.Property(t => t.Lang)
                .HasMaxLength(10);
        }
    }
}
