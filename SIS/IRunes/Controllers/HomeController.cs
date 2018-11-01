namespace IRunes.Controllers
{
    using HTTP.Responses.Contracts;
    using MvcFramework.HttpAttributes;
    using MvcFramework.ViewEngine.Contracts;

    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {            
            return this.View("Index");
        }
    }
}