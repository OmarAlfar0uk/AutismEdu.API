using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class ReportConfig : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.Property(r => r.Frequency)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(r => r.Skills)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(r => r.ClinicalObservations)
                .HasMaxLength(4000);

            builder.Property(r => r.Recommendations)
                .HasMaxLength(4000);

            builder.HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
