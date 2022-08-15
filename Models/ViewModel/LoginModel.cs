﻿using System.ComponentModel.DataAnnotations;

namespace AfterSchool.Models.ViewModel
{
    public class LoginModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
