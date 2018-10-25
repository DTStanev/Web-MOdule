
using MvcFramework.Contracts;
using MvcFramework.Logger;
using MvcFramework.Logger.Contracts;
using MvcFramework.Services;
using MvcFramework.Services.Contracts;
using System;

namespace IRunes
{
    public class StartUp : IMvcApplication
    {
        public void Configure()
        {
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<IHashService, HashSerice>();
            collection.AddService<IUserCookieService, UserCookieService>();
            collection.AddService<ILogger, FileLogger>();
            collection.AddService<ILogger>(() => new FileLogger($"log_{DateTime.Now.Date.Year}.txt"));
        }
    }
}
