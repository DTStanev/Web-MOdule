namespace IRunes.Controllers
{
    using IRunes.Data;
    using MvcFramework;
    using MvcFramework.ViewEngine.Contracts;

    public abstract class BaseController : Controller
    {

        protected BaseController()
        {
            this.Context = new IRunesDbContext();
        }

        protected IRunesDbContext Context { get; set; }
    }
}
