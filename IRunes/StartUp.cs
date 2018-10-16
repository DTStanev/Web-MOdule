
using MvcFramework.Contracts;
using MvcFramework.Logger;
using MvcFramework.Logger.Contracts;
using MvcFramework.Services;
using MvcFramework.Services.Contracts;

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
        }
    }
}
