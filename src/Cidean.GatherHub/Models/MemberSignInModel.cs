using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Models
{
    public class MemberSignInModel
    {
        public bool IsValid { get; set; } = true;
        [Required, EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
