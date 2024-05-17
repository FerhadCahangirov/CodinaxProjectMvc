using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Context
{
    public class CodinaxDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public CodinaxDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public CodinaxDbContext()
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<FutureJobTitle> FutureJobTitles { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Advice> Advices { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    j => j
                        .HasOne<Course>()
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction),
                    j => j
                        .HasOne<Student>()
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction));


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Template>()
                .HasMany(x => x.FutureJobTitles)
                .WithOne(x => x.Template)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FutureJobTitle>()
                .HasOne(x => x.Template)
                .WithMany(x => x.FutureJobTitles)
                .HasForeignKey("TemplateId")
                .OnDelete(DeleteBehavior.NoAction);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TimeZoneInfo aztTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                DateTime currentTimeUtc = DateTime.UtcNow;
                DateTime currentTimeAzt = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc, aztTimeZone);

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Id = Guid.NewGuid();
                    entry.Entity.CreatedDate = currentTimeAzt;
                    entry.Entity.UpdatedDate = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = currentTimeAzt;
                    var modifiedProps = entry.Properties.Where(prop => prop.IsModified && !prop.Metadata.IsPrimaryKey());
                    if (!modifiedProps.Any())
                    {
                        entry.Entity.UpdatedDate = null;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
