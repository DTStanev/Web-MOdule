namespace IRunes.Controllers
{
    using HTTP.Responses.Contracts;
    using MvcFramework.HttpAttributes;

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