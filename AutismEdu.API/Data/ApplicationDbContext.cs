using AutismEdu.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<ChildProfile> ChildProfiles { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<PerformanceRecord> PerformanceRecords { get; set; }
        public DbSet<CommunicationCard> CommunicationCards { get; set; }
        public DbSet<ChatBotLog> ChatBotLogs { get; set; }
        public DbSet<ChildActivity> ChildActivities { get; set; }
        public DbSet<ChildCard> ChildCards { get; set; }
        public DbSet<TTSContent> TTSContents { get; set; }
        public DbSet<Guideline> Guidelines { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ChildLessonLevel> ChildLessonLevels { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
            });

            modelBuilder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
