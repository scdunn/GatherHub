using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class CourseMember
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public class CourseMemberConfig : IEntityTypeConfiguration<CourseMember>
        {
            public void Configure(EntityTypeBuilder<CourseMember> builder)
            {
                builder
                    .HasKey(p => new { p.CourseId, p.MemberId });
                builder
                    .HasOne(p => p.Course)
                    .WithMany(p => p.CourseMembers)
                    .HasForeignKey(p => p.CourseId);
                builder
                    .HasOne(p => p.Member)
                    .WithMany(p => p.CourseMembers)
                    .HasForeignKey(p => p.MemberId);
            }
        }
    }


  
}
