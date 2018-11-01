using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class AlbumTrack
    {
        public Album Album { get; set; }
        public string AlbumId { get; set; }

        public Track Track { get; set; }
        public string TrackId { get; set; }
    }
}
