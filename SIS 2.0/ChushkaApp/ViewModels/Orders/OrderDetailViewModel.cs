using System;
using System.Collections.Generic;
using System.Text;

namespace ChushkaApp.ViewModels.Orders
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }

        public string Customer { get; set; }

        public string Product { get; set; }

        public string OrderedOn { get; set; }
    }
}
