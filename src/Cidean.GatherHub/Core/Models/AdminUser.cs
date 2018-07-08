using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        [Required, MaxLength(25)]
        public string Username { get; set; }

        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        private string Password { get; set; }

        [MaxLength(30), NotMapped]
        public string TempPassword { get; set; }
        

        public AdminUser()
        {

        }

        public AdminUser(string username, string password)
        {
            Username = username;
            SetPassword(password);
        }

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

        public class AdminConfig : IEntityTypeConfiguration<AdminUser>
        {
            public void Configure(EntityTypeBuilder<AdminUser> builder)
            {
                builder.Property(u => u.Password);
            }
        }

    }


}
