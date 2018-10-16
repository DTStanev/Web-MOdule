using HTTP.Requests.Contracts;
using HTTP.Responses.Contracts;
using MvcFramework.Contracts;
using MvcFramework.HttpAttributes;
using MvcFramework.Services;
using MvcFramework.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using WebServer;
using WebServer.Results;
using WebServer.Routing;

namespace MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            var dependencyContainer = new ServiceCollection();
            application.ConfigureServices(dependencyContainer);

            var serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application, dependencyContainer);

            application.Configure();

            var server = new Server(1337, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable, IMvcApplication application, IServiceCollection serviceCollection)
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
                       (request) => ExecuteAction(controller, methodInfo, request, serviceCollection));                   
                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType,
           MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;

            if (controllerInstance == null)
            {
                return new TextResult("Controller not found.",
                    HttpStatusCode.InternalServerError);
            }

            controllerInstance.Request = request;
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            var actionParameters = methodInfo.GetParameters();

            var actionParameterObjects = new List<object>();

            foreach (var actionParameter in actionParameters)
            {
                var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);

                var properties = actionParameter.ParameterType.GetProperties();

                foreach (var propertyInfo in properties)
                {
                    var key = propertyInfo.Name.ToLower();
                    object value = null;

                    if (request.FormData.Any(x => x.Key.ToLower() == key))
                    {
                        value = request.FormData.First(x => x.Key.ToLower() == key).Value.ToString();
                    }
                    else if (request.QueryData.Any(x => x.Key.ToLower() == key))
                    {
                        value = request.QueryData.First(x => x.Key.ToLower() == key).Value.ToString();
                    }

                    propertyInfo.SetMethod.Invoke(instance, new object[] { value });
                }

                actionParameterObjects.Add(instance);
            }

            var httpResponse = methodInfo.Invoke(controllerInstance, actionParameterObjects.ToArray()) as IHttpResponse;          

            return httpResponse;
        }
    }
}
