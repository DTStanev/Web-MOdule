using HTTP.Enums;
using MvcFramework.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using WebServer.Routing;

namespace IRunes
{
    public class StartUp : IMvcApplication
    {
        public void Configure()
        {
            //// {controller}/{action}/{id}
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController() { Request = request }.Index();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request => new UserController() { Request = request }.Login();
            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] = request => new UserController().DoLogin();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request => new UserController().Register();
            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] = request => new UserController().DoRegister();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/logout"] = request => new UserController().Logout();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/all"] = request => new AlbumController().All();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/create"] = request => new AlbumController().Create();
            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/albums/create"] = request => new AlbumController().DoCreate();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/details"] = request => new AlbumController().Details();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/create"] = request => new TrackController().Create();
            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/tracks/create"] = request => new TrackController().DoCreate();
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/details"] = request => new TrackController().Details();
        }

        public void ConfigureServices()
        {
            //TODO: DI Container
        }
    }
}
