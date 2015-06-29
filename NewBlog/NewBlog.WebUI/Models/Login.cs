using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewBlog.WebUI.Models
{
    public class Login
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}