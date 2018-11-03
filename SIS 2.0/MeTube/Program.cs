using SIS.MvcFramework;

namespace MeTube
{
    class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
