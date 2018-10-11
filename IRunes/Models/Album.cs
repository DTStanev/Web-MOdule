using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class Album : BaseModel
    {
        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }
}
