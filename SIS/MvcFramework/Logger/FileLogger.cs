using MvcFramework.Logger.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MvcFramework.Logger
{
    public class FileLogger : ILogger
    {
        private static readonly object LockObject = new object();

        private readonly string fileName;

        public FileLogger(string fileName)
        {
            this.fileName = fileName;
        }

        public void Log(string message)
        {
            lock (LockObject)
            {
                File.AppendAllText(this.fileName, $"[{DateTime.UtcNow}] {message}{Environment.NewLine}");
            }
        }
    }
}
