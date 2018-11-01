using System;
using System.Collections.Generic;
using System.Text;
using HTTP.Enums;

namespace MvcFramework.HttpAttributes
{
    public class HttpGetAttribute : HttpAttribute
    {
        public HttpGetAttribute(string path)
            : base(path)
        {
        }

        public override HttpRequestMethod Method => HttpRequestMethod.Get;
    }
}
