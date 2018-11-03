using MeTube.ViewModels.Home;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeTube.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
                return this.View();
            }

            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            if (user == null)
            {
                var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
                cookie.Delete();
                this.Response.Cookies.Add(cookie);
                return this.Redirect("/");
            }

            var tubes = this.Db.Tubes
                .Select(x => new TubeViewModel
                {
                    Id = x.Id,
                    Author = x.Author,
                    Title = x.Title,
                    YoutubeId = x.YoutubeId
                })
                .ToList();

            var model = new AllTubesViewModel { Tubes = tubes };

            return this.View("/Home/LoggedInIndex", model);
        }
    }
}
