using System;
using System.Collections.Generic;
using System.Text;

namespace MeTube.ViewModels.Tube
{
    public class TubeDetailsViewModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public int Views { get; set; }

        public string Description { get; set; }

        public string YoutubeId { get; set; }
    }
}
