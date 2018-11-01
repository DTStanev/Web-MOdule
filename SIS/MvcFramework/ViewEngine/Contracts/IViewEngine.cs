using System;
using System.Collections.Generic;
using System.Text;

namespace MvcFramework.ViewEngine.Contracts
{
    public interface IViewEngine
    {
        string GetHtml<T>(string viewName, string viewCode, T model, string user);
    }
}
