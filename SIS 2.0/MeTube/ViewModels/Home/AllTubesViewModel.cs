using System;
using System.Collections.Generic;
using System.Text;

namespace MeTube.ViewModels.Home
{
    public class AllTubesViewModel
    {
        public IEnumerable<TubeViewModel> Tubes { get; set; }
    }
}
