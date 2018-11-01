namespace IRunes.Controllers
{
    using HTTP.Responses.Contracts;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using MvcFramework.HttpAttributes;
    using System;
    using System.Linq;
    using ViewModels.Album;

    public class AlbumController : BaseController
    {
        [HttpGet("/albums/all")]
        public IHttpResponse All()
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }
            var user = this.Context.Users.Include(u => u.Albums).FirstOrDefault(u => u.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/users/login");
            }

            var albums = user.Albums.ToArray();

            var albumsInfo = string.Empty;

            return this.View("All", user.Albums.ToArray());
        }

        [HttpGet("/albums/create")]
        public IHttpResponse Create()
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            return this.View("Create");
        }

        [HttpPost("/albums/create")]
        public IHttpResponse DoCreate(DoCreateAlbumViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var user = this.Context.Users.Include(u => u.Albums).FirstOrDefault(u => u.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/");
            }

            var albumName = model.AlbumName;
            var albumCover = model.AlbumCover;
                        
            if (string.IsNullOrWhiteSpace(albumName) || string.IsNullOrWhiteSpace(albumCover))
            {
                return this.View("Create");
            };

            var album = new Album
            {
                Name = albumName,
                Cover = albumCover
            };

            user.Albums.Add(album);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
               return this.ServerError(e.Message);
            }

            return this.Redirect("/albums/all");
        }

        [HttpGet("/albums/details")]        
        public IHttpResponse Details(AlbumDetailsViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var albumId = model.AlbumId;

            var album = this.Context.Albums.Include(a => a.Tracks).FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                return this.Redirect("/albums/all");
            }
            return this.View("Details", album);
        }
    }
}
