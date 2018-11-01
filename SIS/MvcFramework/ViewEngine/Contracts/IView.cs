using System;
using System.Collections.Generic;
using System.Text;

namespace MvcFramework.ViewEngine.Contracts
{
    public interface IView<T>
    {
        string GetHtml(T model, string name);
    }
}
