using MeTube.Data;
using SIS.MvcFramework;

namespace MeTube.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            this.Db = new MeTubeDbContext();
        }

        public MeTubeDbContext Db { get; }
    }
}
