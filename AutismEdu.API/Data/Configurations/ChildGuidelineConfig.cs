using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class ChildGuidelineConfig : IEntityTypeConfiguration<ChildGuideline>
    {
        public void Configure(EntityTypeBuilder<ChildGuideline> builder)
        {
            builder.HasKey(cg => cg.Id);

            builder.HasOne(cg => cg.Child)
                .WithMany()
                .HasForeignKey(cg => cg.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cg => cg.Guideline)
                .WithMany()
                .HasForeignKey(cg => cg.GuidelineId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
