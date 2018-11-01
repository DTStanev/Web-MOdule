using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Models
{
    public class Album : BaseModel
    {
        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price => this.Tracks.Sum(x => x.Price);

        public ICollection<Track> Tracks { get; set; }
    }
}
