using System;
using System.Collections.Generic;
using System.Text;

namespace ChushkaApp.ViewModels.User
{
    public class RegisterInputViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
