using ChushkaApp.Models;

namespace ChushkaApp.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}
