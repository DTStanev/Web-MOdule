namespace IRunes.Controllers
{
    using HTTP.Responses.Contracts;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using ViewModels.Track;
    using MvcFramework.HttpAttributes;

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

            return this.View("Create", model);
        }

        [HttpPost("/tracks/create")]
        public IHttpResponse DoCreate(DoCreateTrackViewModel model)
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

            var trackName = model.TrackName;
            var trackLink = model.TrackLink;
            var trackPrice = model.TrackPrice;

            if (trackPrice < 1)
            {
                return this.BadRequestError("Create", "The price cannot be below 1 dollar", model);
            }

            if (string.IsNullOrWhiteSpace(trackName)
                || string.IsNullOrWhiteSpace(trackLink))
            {
                return this.BadRequestError("Create", "All fields are required!", model);
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
            return this.Redirect($"/albums/details?albumId={albumId}");
        }

        [HttpGet("/tracks/details")]
        public  IHttpResponse Details(TrackDetailsViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var albumId = model.AlbumId;
            var trackId = model.TrackId;

            var track = this.Context.Tracks.FirstOrDefault(t => t.Id == trackId);

            if (track == null)
            {
                return this.Redirect("/albums/all");
            }

            model.Price = track.Price;
            model.Name = track.Name;
            model.Link = track.Link;

            return this.View("Details", model);
        }
    }
}
