using HTTP.Requests.Contracts;
using HTTP.Responses.Contracts;
using IRunes.Extensions;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Results;

namespace IRunes.Controllers
{
    public class TrackController : BaseController
    {
        public IHttpResponse DoCreate(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("Home/Index");
            }

            var albumId = request.QueryData["albumId"].ToString().UrlDecode();

            var album = this.Context.Albums.Include(a => a.Tracks).FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                return new RedirectResult("/albums/all");
            }

            var trackName = request.FormData["trackName"].ToString().UrlDecode();
            var trackLink = request.FormData["trackLink"].ToString().UrlDecode();

            if (!decimal.TryParse(request.FormData["trackPrice"].ToString().UrlDecode(), out var trackPrice)
                || trackPrice < 1
                || string.IsNullOrWhiteSpace(trackName) 
                || string.IsNullOrWhiteSpace(trackLink))
            {
                return new RedirectResult($"/albums/details?id={albumId}");
            }

            var track = new Track
            {
                Name = trackName,
                Link = trackLink.Replace("watch?v=", "embed/"),
                Price = trackPrice,
            };

            album.Tracks.Add(track);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }
            return new RedirectResult($"/albums/details?id={albumId}");
        }

        internal IHttpResponse Create(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return new RedirectResult("/");
            }

            var albumId = request.QueryData["albumId"].ToString().UrlDecode();
            this.ViewBag["@albumId"] = albumId;

            return this.View("Track/Create");
        }

        public  IHttpResponse Details(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return new RedirectResult("/");
            }

            var albumId = request.QueryData["albumId"].ToString().UrlDecode();
            var trackId = request.QueryData["trackId"].ToString().UrlDecode();

            var track = this.Context.Tracks.FirstOrDefault(t => t.Id == trackId);

            if (track == null)
            {
                return new RedirectResult("/albums/all");
            }

            this.ViewBag["@trackName"] = track.Name;
            this.ViewBag["@trackPrice"] = track.Price.ToString("F2");
            this.ViewBag["@videoUrl"] = track.Link;
            this.ViewBag["@albumId"] = albumId;

            return this.View("Track/Details");
        }
    }
}
