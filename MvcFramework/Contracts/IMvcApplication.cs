using System;
using System.Collections.Generic;
using System.Text;
using WebServer.Routing;

namespace MvcFramework.Contracts
{
    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices();
    }
}
