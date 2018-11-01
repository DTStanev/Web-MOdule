using ChushkaApp.ViewModels.Orders;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ChushkaApp.Controllers
{
    public class OrdersController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var orders = this.Db.Orders
                .Select(x => new OrderDetailViewModel
                {
                    Customer = x.Client.Username,
                    Id = x.Id,
                    OrderedOn = x.OrderedOn.ToString(CultureInfo.InvariantCulture),
                    Product = x.Product.Name
                })
                .ToList();

            var model = new AllOrdersViewModel { Orders = orders };

            return this.View(model);
        }
    }
}
