using HTTP.Requests.Contracts;
using HTTP.Responses.Contracts;
using IRunes.Extensions;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WebServer.Results;

namespace IRunes.Controllers
{
    public class AlbumController : BaseController
    {
        public IHttpResponse All(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return new RedirectResult("/");
            }

            this.ViewBag["@allAlbums"] = "You don`t have albums yet.";

            var user = this.Context.Users.Include(u => u.Albums).FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return new RedirectResult("/users/login");
            }

            var albums = user.Albums.ToArray();

            var albumsInfo = string.Empty;

            foreach (var album in albums)
            {
                albumsInfo += $"<a href=\"/albums/details?id={album.Id}\">{album.Name}</a></br>";
            }

            if (!string.IsNullOrWhiteSpace(albumsInfo))
            {
                this.ViewBag["@allAlbums"] = albumsInfo;
            }

            return this.View("Album/Album"); ;
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return new RedirectResult("/");
            }

            return this.View("Album/Create");
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return new RedirectResult("/");
            }

            var user = this.Context.Users.Include(u => u.Albums).FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return new RedirectResult("/");
            }

            var albumName = request.FormData["albumName"].ToString().UrlDecode();
            var albumCover = request.FormData["albumCover"].ToString().UrlDecode();
                        
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

            return new RedirectResult("/albums/all");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return new RedirectResult("/");
            }

            var albumId = request.QueryData["id"].ToString().UrlDecode();
            var album = this.Context.Albums.Include(a => a.Tracks).FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                return new RedirectResult("/albums/all");
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
                    tracksInfo += $"<li>{i}.<a href=\"/tracks/details?id={currentTrack.Id}&albumId={album.Id}\">{currentTrack.Name}</a></li>";
                }
                this.ViewBag["@allTracks"] = tracksInfo;
            }

            return this.View("Album/Details");
        }
    }
}
