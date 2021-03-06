﻿using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class LoginWithCredentialsRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
