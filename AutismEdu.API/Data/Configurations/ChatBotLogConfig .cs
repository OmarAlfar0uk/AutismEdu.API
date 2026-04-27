using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutismEdu.API.Data.Configurations
{
    public class ChatBotLogConfig : IEntityTypeConfiguration<ChatBotLog>
    {
        public void Configure(EntityTypeBuilder<ChatBotLog> builder)
        {
            builder.Property(c => c.Question)
                .IsRequired();

            builder.Property(c => c.Answer)
                .IsRequired();

            builder.HasOne(c => c.Child)
                .WithMany(ch => ch.ChatBotLogs)
                .HasForeignKey(c => c.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}