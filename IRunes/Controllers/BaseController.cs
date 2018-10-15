namespace IRunes.Controllers
{
    using IRunes.Data;
    using MvcFramework;

    public abstract class BaseController : Controller
    {

        public BaseController()
        {
            this.Context = new IRunesDbContext();
        }

        protected IRunesDbContext Context { get; set; }
    }
}
