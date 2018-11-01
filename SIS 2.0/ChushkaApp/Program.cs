using SIS.MvcFramework;
using System;

namespace ChushkaApp
{
    public class Program
    {
        public static void Main()
        {
            WebHost.Start(new Startup());
        }
    }
}
