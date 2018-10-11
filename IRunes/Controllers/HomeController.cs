using HTTP.Requests.Contracts;
using HTTP.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username != null)
            {
                this.ViewBag["@@username"] = username;
            }
            return this.View("Home/Index");
        }
    }
}
