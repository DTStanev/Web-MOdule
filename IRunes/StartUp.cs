using HTTP.Enums;
using IRunes.Controllers;
using System;
using WebServer;
using WebServer.Routing;

namespace IRunes
{
    public class StartUp
    {
        public static void Main()
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request
                => new HomeController().Index(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request => new UserController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] = request => new UserController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request => new UserController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] = request => new UserController().DoRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/logout"] = request => new UserController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/all"] = request => new AlbumController().All(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/create"] = request => new AlbumController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/albums/create"] = request => new AlbumController().DoCreate(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/details"] = request => new AlbumController().Details(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/create"] = request => new TrackController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/tracks/create"] = request => new TrackController().DoCreate(request);


            Server server = new Server(1337, serverRoutingTable);

            server.Run();
        }
    }
}
;