using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foreman.Shared.Data.Courses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Foreman.Server.Data
{
    public class ApplicationContext : IdentityDbContext<Shared.Data.Identity.UserProfile>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }

        public ApplicationContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            CourseFluentApi(modelBuilder);
            CourseCategoryFluentApi(modelBuilder);
            
            base.OnModelCreating(modelBuilder);

        }

        protected void CourseFluentApi(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasMany(c=>c.CourseModules)
                .WithOne(cm=> cm.Course);
            modelBuilder.Entity<Course>().HasMany(c=> c.CourseSections)
                .WithOne(cs=>cs.Course);
            modelBuilder.Entity<Course>().HasOne(c=> c.Category)
                .WithMany(cc=>cc.Courses);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
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
            modelBuilder.Entity<CourseCategory>().HasOne(cc => cc.ParentCategory);
            modelBuilder.Entity<CourseCategory>().Property(c => c.IsVisible).ValueGeneratedOnAdd();
            modelBuilder.Entity<CourseCategory>().Property(c => c.CreatedDate).ValueGeneratedOnAdd();
            modelBuilder.Entity<CourseCategory>().Property(c => c.Name).HasMaxLength(100);
            modelBuilder.Entity<CourseCategory>().Property(c => c.Description).HasMaxLength(255);
        }
    }
}
