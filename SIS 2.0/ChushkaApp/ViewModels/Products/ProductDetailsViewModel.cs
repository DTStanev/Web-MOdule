using ChushkaApp.Models;

namespace ChushkaApp.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }

        public decimal Price { get; set; }
    }
}
