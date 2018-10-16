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

        public FileLogger()
        {
            this.fileName = "log.txt";
        }

        public void Log(string message)
        {
            lock (LockObject)
            {
                File.AppendAllText(this.fileName, message + Environment.NewLine);
            }
        }
    }
}
