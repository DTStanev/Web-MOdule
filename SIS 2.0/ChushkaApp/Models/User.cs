using System.Collections.Generic;

namespace ChushkaApp.Models
{
    public class User
    {
        public User()
        {
            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public  string Email { get; set; }

        public Role Role { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
