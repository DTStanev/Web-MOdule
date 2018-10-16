using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.ViewModels
{
    public class DoRegisterInputViewModel
    {
        public string Username { get; private set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
    }
}
