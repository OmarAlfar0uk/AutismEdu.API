using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(a => a.FocusArea)
                .HasMaxLength(200);

            builder.HasOne(a => a.Specialist)
                .WithMany()
                .HasForeignKey(a => a.SpecialistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Child)
                .WithMany()
                .HasForeignKey(a => a.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
