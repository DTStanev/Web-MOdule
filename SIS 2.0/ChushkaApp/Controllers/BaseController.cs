using ChushkaApp.Data;
using SIS.MvcFramework;

namespace ChushkaApp.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            this.Db = new ChushkaDBContext();
        }

        public ChushkaDBContext Db { get; }
    }
}
