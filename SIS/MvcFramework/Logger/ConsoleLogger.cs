using MvcFramework.Logger.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MvcFramework.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
