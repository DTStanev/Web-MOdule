using System;
using System.Collections.Generic;
using System.Text;

namespace MvcFramework.Logger.Contracts
{
    public interface ILogger
    {
        void Log(string message);
    }
}
