using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class GuidelineConfig : IEntityTypeConfiguration<Guideline>
    {
        public void Configure(EntityTypeBuilder<Guideline> builder)
        {
            builder.Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(g => g.Description)
                .HasMaxLength(2000);
        }
    }
}
