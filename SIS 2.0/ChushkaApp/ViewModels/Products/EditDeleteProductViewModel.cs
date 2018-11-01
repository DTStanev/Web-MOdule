using System;
using System.Collections.Generic;
using System.Text;

namespace ChushkaApp.ViewModels.Products
{
    public class EditDeleteProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }
    }
}
