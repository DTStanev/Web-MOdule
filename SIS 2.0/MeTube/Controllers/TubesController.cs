using MeTube.Models;
using MeTube.ViewModels.Tube;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeTube.Controllers
{
    public class TubesController :BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var tube = this.Db.Tubes.Find(id);

            if (tube == null)
            {
                return this.BadRequestError("There is no such tube.");
            }

            tube.Views++;

            var model = tube.To<TubeDetailsViewModel>();

            this.Db.SaveChanges();
            return this.View(model);
        }

        [Authorize]
        public IHttpResponse Upload()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Upload(UploadInputViewModel model)
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            if (user == null)
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return this.BadRequestErrorWithView("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Author))
            {
                return this.BadRequestErrorWithView("Author is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Link))
            {
                return this.BadRequestErrorWithView("Youtube Link is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                return this.BadRequestErrorWithView("Description is required.");
            }

            var urlParts = model.Link.Split("?");

            
            var urlData = urlParts[1].Split(new string[] { "=", "&" }, StringSplitOptions.RemoveEmptyEntries);

            var youtubeId = urlData[1];

            var tube = new Tube
            {
                Title = model.Title,
                Author = model.Author, 
                Description = model.Description,
                Uploader = user,
                YoutubeId = youtubeId
            };

            this.Db.Tubes.Add(tube);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }
    }
}
