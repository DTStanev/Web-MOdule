using ChushkaApp.ViewModels.Home;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChushkaApp.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var products = this.Db.Products
                    .Select(x => new ProductViewModel
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Name = x.Name,
                        Price = x.Price,
                    })
                    .ToList();

                var model = new IndexViewModel { Products = products };

                return this.View("Home/IndexLoggedIn", model);
                
            }
            return this.View();
        }
    }
}
