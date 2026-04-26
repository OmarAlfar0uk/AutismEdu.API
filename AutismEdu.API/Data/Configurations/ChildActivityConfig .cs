using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class ChildActivityConfig : IEntityTypeConfiguration<ChildActivity>
    {
        public void Configure(EntityTypeBuilder<ChildActivity> builder)
        {
            builder.HasKey(ca => new { ca.ChildId, ca.ActivityId });

            builder.HasOne(ca => ca.Child)
                .WithMany(c => c.ChildActivities)
                .HasForeignKey(ca => ca.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Activity)
                .WithMany(a => a.ChildActivities)
                .HasForeignKey(ca => ca.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}