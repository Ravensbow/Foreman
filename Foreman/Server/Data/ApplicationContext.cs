using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foreman.Shared.Data.Courses;
using Foreman.Shared.Data.Plugin;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using Foreman.Shared.Data.Identity;

namespace Foreman.Server.Data
{
    public class ApplicationContext : KeyApiAuthorizationDbContext<Shared.Data.Identity.UserProfile,Shared.Data.Identity.Role,int>
    {
        public DbSet<ForemanFile> Files { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }
        public DbSet<Plugin> Plugins { get; set; }

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Course data
            CourseFluentApi(modelBuilder);
            CourseCategoryFluentApi(modelBuilder);
            CourseSectionFlutenApi(modelBuilder);
            CourseModuleFlutenApi(modelBuilder);
            AssigmentFlutenApi(modelBuilder);
            CategoryAssigmentFlutenApi(modelBuilder);
            //Identity data
            UserProfileFluentApi(modelBuilder);
            InstitutionFluentApi(modelBuilder);
            UserAssigmentFluentApi(modelBuilder);
            //Plugins
            PluginFluentApi(modelBuilder);
            //ForemanFile
            FileFluentApi(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #region Course FluentApi

        protected void FileFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForemanFile>().HasKey(f => f.Id);
            modelBuilder.Entity<ForemanFile>().Property(f => f.ContentHash)
                .HasColumnType("VARCHAR(255)");
            modelBuilder.Entity<ForemanFile>().Property(f => f.PathNameHash)
                .HasColumnType("VARCHAR(255)");
            modelBuilder.Entity<ForemanFile>().Property(f => f.Component)
                .HasColumnType("VARCHAR(255)");
            modelBuilder.Entity<ForemanFile>().Property(f => f.MimeType)
                .HasColumnType("VARCHAR(100)");

        }

        protected void CourseFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasMany(c => c.CourseModules)
                .WithOne(cm => cm.Course);
            modelBuilder.Entity<Course>().HasMany(c => c.CourseSections)
                .WithOne(cs => cs.Course);
            modelBuilder.Entity<Course>().HasOne(c => c.Category)
                .WithMany(cc => cc.Courses);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Course>().HasMany(c => c.Assigments)
                .WithOne(c => c.Course);
            modelBuilder.Entity<Course>().HasOne(c => c.Institution)
                .WithMany(i => i.Courses);

            modelBuilder.Entity<Course>().Property(c => c.IsVisible).ValueGeneratedOnAdd();
            modelBuilder.Entity<Course>().Property(c => c.CreatedDate).ValueGeneratedOnAdd();
            modelBuilder.Entity<Course>().Property(c => c.ShortName).HasMaxLength(50);
            modelBuilder.Entity<Course>().Property(c => c.FullName).HasMaxLength(255);
        }

        protected void CourseCategoryFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseCategory>().HasKey(cc => cc.Id);
            modelBuilder.Entity<CourseCategory>().HasMany(cc => cc.Courses)
                .WithOne(c => c.Category);
            modelBuilder.Entity<CourseCategory>().HasMany(cc => cc.CategoryAssigments)
                .WithOne(ca => ca.Category);
            modelBuilder.Entity<CourseCategory>().HasOne(cc => cc.ParentCategory);
            modelBuilder.Entity<CourseCategory>().HasOne(cc => cc.Institution)
                .WithMany(cc => cc.CourseCategories);
            
            modelBuilder.Entity<CourseCategory>().Property(c => c.IsVisible).ValueGeneratedOnAdd();
            modelBuilder.Entity<CourseCategory>().Property(c => c.CreatedDate).ValueGeneratedOnAdd();
            modelBuilder.Entity<CourseCategory>().Property(c => c.Name).HasMaxLength(100);
            modelBuilder.Entity<CourseCategory>().Property(c => c.Description).HasMaxLength(255);
        }
        protected void CourseSectionFlutenApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseSection>().HasKey(cs => cs.Id);
            modelBuilder.Entity<CourseSection>().HasOne(cs => cs.Course)
                .WithMany(c => c.CourseSections);
            modelBuilder.Entity<CourseSection>().HasMany(cs => cs.CourseModules)
                .WithOne(cm => cm.CourseSection);

            modelBuilder.Entity<CourseSection>().Property(cs => cs.IsVisible).ValueGeneratedOnAdd();
            modelBuilder.Entity<CourseSection>().Property(cs => cs.Name).HasMaxLength(255);
        }
        protected void CourseModuleFlutenApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseModule>().HasKey(cm => cm.Id);
            modelBuilder.Entity<CourseModule>().HasOne(cm => cm.Course)
                .WithMany(c => c.CourseModules);
            modelBuilder.Entity<CourseModule>().HasOne(cm => cm.CourseSection)
                .WithMany(cs => cs.CourseModules);

            modelBuilder.Entity<CourseModule>().Property(cm => cm.IsVisible).ValueGeneratedOnAdd();
        }
        protected void AssigmentFlutenApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assigment>().HasKey(a => a.Id);
            modelBuilder.Entity<Assigment>().HasOne(cm => cm.Course)
                .WithMany(c => c.Assigments);
            modelBuilder.Entity<Assigment>().HasMany(a => a.UserAssigments)
                .WithOne(ua => ua.Assigment);

        }
        protected void CategoryAssigmentFlutenApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryAssigment>().HasKey(ca => new { ca.UserId, ca.RoleId, ca.CateogryId });
            modelBuilder.Entity<CategoryAssigment>().HasOne(ca => ca.UserProfile)
                .WithMany(up => up.CategoryAssigments);
            modelBuilder.Entity<CategoryAssigment>().HasOne(ca => ca.Category)
                .WithMany(cc => cc.CategoryAssigments);
        }
        #endregion
        #region Identity FluentApi
        protected void UserProfileFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasOne(up => up.Institution)
                .WithMany(i => i.Members);
            modelBuilder.Entity<UserProfile>().HasMany(up => up.UserAssigments)
                .WithOne(ua => ua.UserProfile);
            modelBuilder.Entity<UserProfile>().HasMany(up => up.CategoryAssigments)
                .WithOne(ua => ua.UserProfile);
            modelBuilder.Entity<UserProfile>().HasOne(up => up.OwnedInstitution)
                .WithOne(i => i.Owner)
                .HasForeignKey<UserProfile>(up=>up.OwnedInstitutionId);
        }
        protected void InstitutionFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Institution>().HasKey(i => i.Id);
            modelBuilder.Entity<Institution>().HasMany(i => i.Members)
                .WithOne(up => up.Institution);
            modelBuilder.Entity<Institution>().HasMany(i => i.Courses)
                .WithOne(c => c.Institution);
            modelBuilder.Entity<Institution>().HasMany(i => i.CourseCategories)
                .WithOne(cc => cc.Institution);
            modelBuilder.Entity<Institution>().HasOne(i => i.Owner)
                .WithOne(up => up.OwnedInstitution)
                .HasForeignKey<Institution>(i => i.OwnerId);
        }
        protected void UserAssigmentFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAssigment>().HasOne(ua => ua.UserProfile)
                .WithMany(up => up.UserAssigments);
        }
        #endregion
        protected void PluginFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plugin>().HasKey(p => p.Id);            
        }

    }
}
