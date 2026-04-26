using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class CommunicationCardConfig : IEntityTypeConfiguration<CommunicationCard>
    {
        public void Configure(EntityTypeBuilder<CommunicationCard> builder)
        {
            builder.Property(c => c.ImageUrl)
                .IsRequired();

            builder.Property(c => c.Title)
                .IsRequired();
        }
    }
}
