using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cidean.GatherHub.Core.Data
{
    public class HubContext:DbContext
    {
        public HubContext(DbContextOptions<HubContext> options): base(options){}

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }      
        public DbSet<CourseMember> CourseMembers { get; set; }
        public DbSet<ActivityLogItem> ActivityLogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Course Configuration
            modelBuilder.Entity<Course>()
                    .HasOne(p => p.Instructor)
                    .WithMany()
                    .HasForeignKey(p => p.InstructorId);

            //Course Member Configuration
            modelBuilder.Entity<CourseMember>()
                    .HasKey(p => new { p.CourseId, p.MemberId });

            modelBuilder.Entity<CourseMember>()
                .HasOne(p => p.Course)
                .WithMany(p => p.CourseMembers)
                .HasForeignKey(p => p.CourseId);

            modelBuilder.Entity<CourseMember>()
                .HasOne(p => p.Member)
                .WithMany(p => p.CourseMembers)
                .HasForeignKey(p => p.MemberId);

            modelBuilder.ApplyConfiguration(new AdminUser.AdminConfig());
            modelBuilder.ApplyConfiguration(new Member.MemberConfig());
            

        }


    }

}
