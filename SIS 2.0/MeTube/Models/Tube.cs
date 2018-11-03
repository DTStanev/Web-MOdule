using System;
using System.Collections.Generic;
using System.Text;

namespace MeTube.Models
{
    public class Tube
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string YoutubeId { get; set; }

        public int Views { get; set; }

        public int UploaderId { get; set; }
        public User Uploader { get; set; }
    }
}
