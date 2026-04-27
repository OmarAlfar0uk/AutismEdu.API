using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class PerformanceRecordConfig : IEntityTypeConfiguration<PerformanceRecord>
    {
        public void Configure(EntityTypeBuilder<PerformanceRecord> builder)
        {
            builder.Property(p => p.Score)
                .IsRequired();

            builder.HasOne(p => p.Child)
                .WithMany(c => c.PerformanceRecords)
                .HasForeignKey(p => p.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Lesson)
                .WithMany(l => l.PerformanceRecords)
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
