using System;
using System.Collections.Generic;
using System.Text;

namespace ChushkaApp.ViewModels.Orders
{
    public class AllOrdersViewModel
    {
        public IEnumerable<OrderDetailViewModel> Orders { get; set; }
    }
}
