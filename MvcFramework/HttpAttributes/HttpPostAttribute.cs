using System;
using System.Collections.Generic;
using System.Text;
using HTTP.Enums;

namespace MvcFramework.HttpAttributes
{
    public class HttpPostAttribute : HttpAttribute
    {
        public HttpPostAttribute(string path)
            : base(path)
        {
        }

        public override HttpRequestMethod Method => HttpRequestMethod.Post;
    }
}
