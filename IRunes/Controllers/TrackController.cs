using HTTP.Responses.Contracts;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using MvcFramework.Extensions;
using MvcFramework.HttpAttributes;
using System;
using System.Linq;

namespace IRunes.Controllers
{
    public class TrackController : BaseController
    {

        [HttpPost("/tracks/create")]
        public IHttpResponse DoCreate()
        {            
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var albumId = this.Request.QueryData["albumId"].ToString().UrlDecode();

            var album = this.Context.Albums.Include(a => a.Tracks).FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                return this.Redirect("/albums/all");
            }

            var trackName = this.Request.FormData["trackName"].ToString().UrlDecode();
            var trackLink = this.Request.FormData["trackLink"].ToString().UrlDecode();

            if (!decimal.TryParse(this.Request.FormData["trackPrice"].ToString().UrlDecode(), out var trackPrice)
                || trackPrice < 1
                || string.IsNullOrWhiteSpace(trackName) 
                || string.IsNullOrWhiteSpace(trackLink))
            {
                return this.Redirect($"/albums/details?id={albumId}");
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
               // return this.ServerError(e.Message);
            }
            return this.Redirect($"/albums/details?id={albumId}");
        }

        [HttpGet("/tracks/create")]
        public IHttpResponse Create()
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var albumId = this.Request.QueryData["albumId"].ToString().UrlDecode();
            this.ViewBag["@albumId"] = albumId;

            return this.View("Create");
        }

        [HttpGet("/tracks/details")]
        public  IHttpResponse Details()
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var albumId = this.Request.QueryData["albumId"].ToString().UrlDecode();
            var trackId = this.Request.QueryData["trackId"].ToString().UrlDecode();

            var track = this.Context.Tracks.FirstOrDefault(t => t.Id == trackId);

            if (track == null)
            {
                return this.Redirect("/albums/all");
            }

            this.ViewBag["@trackName"] = track.Name;
            this.ViewBag["@trackPrice"] = track.Price.ToString("F2");
            this.ViewBag["@videoUrl"] = track.Link;
            this.ViewBag["@albumId"] = albumId;

            return this.View("Details");
        }
    }
}
