using HTTP.Requests.Contracts;
using HTTP.Responses.Contracts;
using MvcFramework.HttpAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            if (this.User != null)
            {
                this.ViewBag["@@username"] = this.User;
            }
            return this.View("Index");
        }
    }
}
