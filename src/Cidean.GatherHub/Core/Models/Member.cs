using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class Member
    {
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string FirstName { get; set; }
        [Required, MaxLength(30)]
        public string LastName { get; set; }
        [Required, EmailAddress, MaxLength(255) ]
        public string EmailAddress { get; set; }
        private string Password { get; set; }
        public string TempPassword { get; set; }
        

        public void SetPassword(string value)
        {
            //TODO: Set iterations somewhere else.
            Password = Helpers.Hasher.Generate(value, 100);
        }

        public bool IsValidPassword(string value)
        {
            if (Helpers.Hasher.IsValid(value, Password))
                return true;
            return false;
        }

        public class MemberConfig : IEntityTypeConfiguration<Member>
        {
            public void Configure(EntityTypeBuilder<Member> builder)
            {
                builder.Property(u => u.Password);
            }
        }
    }
}
