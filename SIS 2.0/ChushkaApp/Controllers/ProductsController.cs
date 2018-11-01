using ChushkaApp.Models;
using ChushkaApp.ViewModels.Products;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChushkaApp.Controllers
{
    public class ProductsController : BaseController
    {
        [Authorize]
        public IHttpResponse Order(int id)
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            if (user == null)
            {
                return this.BadRequestError("Invalid User!");
            }

            var productId = this.Db.Products.FirstOrDefault(x => x.Id == id).Id;

            if (productId == 0)
            {
                return BadRequestError("Product not found");
            }

            var order = new Order { Client = user, ProductId = productId };
            this.Db.Orders.Add(order);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Delete(int id)
        {
            var model = this.Db.Products.Select(x
                => new EditDeleteProductViewModel
                {
                    Id = id,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description,
                    Type = x.Type.ToString()
                })
                .FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return this.BadRequestErrorWithView("Invalid product.", id);
            }

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Delete(int id, string name)
        {
            var product = this.Db.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestErrorWithView("Invalid product.", id);
            }

            this.Db.Products.Remove(product);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var model = this.Db.Products.Select(x
                => new EditDeleteProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description,
                    Type = x.Type.ToString()
                })
                .FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return this.BadRequestErrorWithView("Invalid product.", id);
            }

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(EditDeleteProductViewModel model)
        {
            var product = this.Db.Products.FirstOrDefault(x => x.Id == model.Id);

            if (product == null)
            {
                return this.BadRequestErrorWithView("Invalid product.", model.Id);
            }

            if (!Enum.TryParse<Models.Type>(model.Type, out var type))
            {
                return this.BadRequestError("Invalid product type.");
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.Type = type;

            Db.SaveChanges();
            return this.Redirect($"/products/details?id={product.Id}");
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(CreateProductViewModel model)
        {
            if (!Enum.TryParse<ChushkaApp.Models.Type>(model.Type, true, out var type))
            {
                return this.BadRequestErrorWithView("Invalid type.");
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Type = type
            };

            this.Db.Products.Add(product);
            Db.SaveChanges();

            return this.Redirect($"/products/details?id={product.Id}");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var model = this.Db.Products
                .Select(x => new ProductDetailsViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    Type = x.Type
                })
                .FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return this.BadRequestErrorWithView("/Home/IndexLoggedIn", "ProductNotFound");
            }

            return this.View(model);
        }        
    }
}
