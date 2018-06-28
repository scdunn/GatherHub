using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Models
{
    public class SignInModel
    {
        public bool IsValid { get; set; } = true;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
