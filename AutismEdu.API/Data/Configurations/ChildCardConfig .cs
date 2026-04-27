using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class ChildCardConfig : IEntityTypeConfiguration<ChildCard>
    {
        public void Configure(EntityTypeBuilder<ChildCard> builder)
        {
            builder.HasKey(cc => new { cc.ChildId, cc.CardId });

            builder.HasOne(cc => cc.Child)
                .WithMany(c => c.ChildCards)
                .HasForeignKey(cc => cc.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Card)
                .WithMany(a => a.ChildCards)
                .HasForeignKey(cc => cc.CardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}