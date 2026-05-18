using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class ChildProfileConfig : IEntityTypeConfiguration<ChildProfile>
    {
        public void Configure(EntityTypeBuilder<ChildProfile> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(c => c.User)
                .WithMany(u => u.ChildProfiles)
                .HasForeignKey(c => c.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
