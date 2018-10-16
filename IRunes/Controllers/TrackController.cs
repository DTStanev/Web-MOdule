using HTTP.Responses.Contracts;
using IRunes.Models;
using IRunes.ViewModels;
using IRunes.ViewModels.Track;
using Microsoft.EntityFrameworkCore;
using MvcFramework.Extensions;
using MvcFramework.HttpAttributes;
using System;
using System.Linq;

namespace IRunes.Controllers
{
    public class TrackController : BaseController
    {
        [HttpGet("/tracks/create")]
        public IHttpResponse Create(CreateTrackViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var albumId = model.AlbumId;
            this.ViewBag["@albumId"] = albumId;

            return this.View("Create");
        }

        [HttpPost("/tracks/create")]
        public IHttpResponse DoCreate(DoCreateTrackViewModel model)
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

            var trackName = model.TrackName.UrlDecode();
            var trackLink = model.TrackLink.UrlDecode();

            if (!decimal.TryParse(model.TrackPrice.UrlDecode(), out var trackPrice)
                || trackPrice < 1
                || string.IsNullOrWhiteSpace(trackName)
                || string.IsNullOrWhiteSpace(trackLink))
            {
                return this.Redirect($"/albums/details?albumId={albumId}");
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
            return this.Redirect($"/albums/details?albumId={albumId}");
        }

        [HttpGet("/tracks/details")]
        public  IHttpResponse Details(TrackDetailsViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var albumId = model.AlbumId.UrlDecode();
            var trackId = model.TrackId.UrlDecode();

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
