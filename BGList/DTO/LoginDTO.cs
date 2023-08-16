﻿using System.ComponentModel.DataAnnotations;

namespace BGList.DTO
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
