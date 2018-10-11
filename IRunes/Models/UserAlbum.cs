using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class UserAlbum
    {
        public User User { get; set; }
        public string UserId { get; set; }

        public Album Album { get; set; }
        public string AlbumId { get; set; }
    }
}
