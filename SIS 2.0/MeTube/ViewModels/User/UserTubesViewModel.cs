using System;
using System.Collections.Generic;
using System.Text;

namespace MeTube.ViewModels.User
{
    public class UserTubesViewModel
    {
        public string Email { get; set; }
        
        public IEnumerable<UserTubeViewModel> Tubes { get; set; }
    }
}
