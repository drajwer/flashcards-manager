using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlashcardsManager.Core.ViewModels
{
    public class RegisterFormViewModel
    {
        [Required]
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

    }
}
