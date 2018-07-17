// Copyright 2018 Cidean and Chris Dunn.  All rights reserved.

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
    /// <summary>
    /// Represents and administrative user of the back-end management
    /// system.
    /// </summary>
    public class AdminUser
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Username { get; set; }

        [Required, MaxLength(25)]
        public string FirstName { get; set; }

        [Required, MaxLength(30)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        private string Password { get; set; }

        [MaxLength(30), NotMapped]
        public string TempPassword { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the Admin User
        /// </summary>
        public AdminUser()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Admin User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public AdminUser(string username, string password)
        {
            Username = username;
            SetPassword(password);
        }

        /// <summary>
        /// Set the users hashed value of the password from the 
        /// clear text password.
        /// </summary>
        /// <param name="value"></param>
        public void SetPassword(string value)
        {
            //TODO: Set iterations somewhere else.
            Password = Helpers.Hasher.Generate(value, 100);
        }

        /// <summary>
        /// Returns true if hashed value equals existing password hash.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
