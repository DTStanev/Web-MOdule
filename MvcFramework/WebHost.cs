using HTTP.Requests.Contracts;
using HTTP.Responses.Contracts;
using MvcFramework.Contracts;
using MvcFramework.HttpAttributes;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using WebServer;
using WebServer.Results;
using WebServer.Routing;

namespace MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            application.ConfigureServices();

            var serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application);

            application.Configure();

            var server = new Server(1337, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable, IMvcApplication application)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(myType => myType.IsClass
                                 && !myType.IsAbstract
                                 && myType.IsSubclassOf(typeof(Controller)));
            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(
                    method => method.CustomAttributes.Any(
                        ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttribute)methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(ca =>
                            ca.GetType().IsSubclassOf(typeof(HttpAttribute)));
                    if (httpAttribute == null)
                    {
                        continue;
                    }
                    routingTable.Add(httpAttribute.Method, httpAttribute.Path,
                       (request) => ExecuteAction(controller, methodInfo, request));                   
                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType,
           MethodInfo methodInfo, IHttpRequest request)
        {
            var controllerInstance = Activator.CreateInstance(controllerType) as Controller;
            if (controllerInstance == null)
            {
                return new TextResult("Controller not found.",
                    HttpStatusCode.InternalServerError);
            }
            controllerInstance.Request = request;
            var httpResponse = methodInfo.Invoke(controllerInstance, new object[] { }) as IHttpResponse;
            return httpResponse;
        }
    }
}
