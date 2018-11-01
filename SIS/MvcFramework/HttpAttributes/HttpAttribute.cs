using HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MvcFramework.HttpAttributes
{
    public abstract class HttpAttribute : Attribute
    {
        protected HttpAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; }
        public abstract HttpRequestMethod Method { get; }
    }
}
