using HTTP.Responses.Contracts;
using IRunes.Models;
using IRunes.ViewModels;
using Microsoft.EntityFrameworkCore;
using MvcFramework.Extensions;
using MvcFramework.HttpAttributes;
using System;
using System.Linq;

namespace IRunes.Controllers
{
    public class AlbumController : BaseController
    {
        [HttpGet("/albums/all")]
        public IHttpResponse All()
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            this.ViewBag["@allAlbums"] = "You don`t have albums yet.";

            var user = this.Context.Users.Include(u => u.Albums).FirstOrDefault(u => u.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/users/login");
            }

            var albums = user.Albums.ToArray();

            var albumsInfo = string.Empty;

            foreach (var album in albums)
            {
                albumsInfo += $"<a href=\"/albums/details?albumId={album.Id}\">{album.Name}</a></br>";
            }

            if (!string.IsNullOrWhiteSpace(albumsInfo))
            {
                this.ViewBag["@allAlbums"] = albumsInfo;
            }

            return this.View("Album"); ;
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

        [HttpPost("albums/create")]
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

            var albumName = model.AlbumName.UrlDecode();
            var albumCover = model.AlbumCover.UrlDecode();
                        
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
               // return this.ServerError(e.Message);
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

            var albumId = model.AlbumId.UrlDecode();

            var album = this.Context.Albums.Include(a => a.Tracks).FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                return this.Redirect("/albums/all");
            }

            this.ViewBag["@anyTracks"] = "none";

            var albumCover = album.Cover;
            var albumName = album.Name;
            var albumPriceAfterDiscount = album.Tracks.Sum(t => t.Price) * 0.87m;
            this.ViewBag["@albumId"] = albumId;


            this.ViewBag["@albumCover"] = albumCover;
            this.ViewBag["@albumName"] = albumName;
            this.ViewBag["@albumPrice"] = albumPriceAfterDiscount.ToString("F2");
            
            var tracks = album.Tracks.ToArray();
            var tracksInfo = string.Empty;

            if (tracks.Length > 0)
            {
                this.ViewBag["@anyTracks"] = "";
                this.ViewBag["@noTracks"] = "none";

                for (int i = 1; i <= tracks.Length; i++)
                {
                    var currentTrack = tracks[i - 1];
                    tracksInfo += $"<li>{i}.<a href=\"/tracks/details?trackId={currentTrack.Id}&albumId={album.Id}\">{currentTrack.Name}</a></li>";
                }
                this.ViewBag["@allTracks"] = tracksInfo;
            }

            return this.View("Details");
        }
    }
}
