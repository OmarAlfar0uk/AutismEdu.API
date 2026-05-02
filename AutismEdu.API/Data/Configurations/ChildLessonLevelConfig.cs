using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class ChildLessonLevelConfig : IEntityTypeConfiguration<ChildLessonLevel>
    {
        public void Configure(EntityTypeBuilder<ChildLessonLevel> builder)
        {
            builder.Property(c => c.Level)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.MasteryPercent)
                .IsRequired();

            builder.Property(c => c.Notes)
                .HasMaxLength(1000);

            builder.HasIndex(c => new { c.ChildId, c.LessonId })
                .IsUnique();

            builder.HasOne(c => c.Child)
                .WithMany()
                .HasForeignKey(c => c.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Lesson)
                .WithMany()
                .HasForeignKey(c => c.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
