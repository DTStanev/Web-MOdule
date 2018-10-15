using MvcFramework;

namespace IRunes
{
    public class Launcher
    {
        public static void Main()
        {          
            WebHost.Start(new StartUp());
        }
    }
}
;