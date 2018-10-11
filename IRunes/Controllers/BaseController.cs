namespace IRunes.Controllers
{
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using IRunes.Data;
    using IRunes.Services;
    using IRunes.Services.Contracts;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using WebServer.Results;

    public abstract class BaseController
    {
        private const string ViewPath = "../../../Views/{0}.html";

        public BaseController()
        {
            this.Context = new IRunesDbContext();
            this.UserCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected bool IsUserAuthenticated { get; set; } = false;

        protected IRunesDbContext Context { get; set; }

        protected IUserCookieService UserCookieService { get; set; }

        protected Dictionary<string, string> ViewBag { get; set; }

        protected IHttpResponse View(string viewName)
        {
            var layout = File.ReadAllText(string.Format(ViewPath, "_layout"));
            var content = File.ReadAllText(string.Format(ViewPath, viewName));
            this.SetViewBagParameters(content);
            var allcontent = this.GetFullViewContent(layout);
            return new HtmlResult(allcontent, HttpStatusCode.OK);

        }

        private string GetFullViewContent(string content)
        {
            content = content.Replace("@RenderBody()", ViewBag["@RenderBody()"]);

            foreach (var viewbagKey in this.ViewBag.Keys)
            {
                if (content.Contains(viewbagKey))
                {
                    content = content.Replace(viewbagKey, this.ViewBag[viewbagKey]);
                }
            }

            return content;
        }

        protected void SetViewBagParameters(string content)
        {
            var bodyKey = "@RenderBody()";
            var nonAuthenticatedKey = "@IsNotAuthenticated";
            var authenticatedKey = "@IsAuthenticated";

            this.ViewBag[bodyKey] = content;
            this.ViewBag[nonAuthenticatedKey] = "";
            this.ViewBag[authenticatedKey] = "";

            if (!IsUserAuthenticated)
            {
                this.ViewBag[authenticatedKey] = "none";
            }
            else
            {
                this.ViewBag[nonAuthenticatedKey] = "none";
            }
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpStatusCode.InternalServerError);
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-IRunes"))
            {
                return null;
            }

            this.IsUserAuthenticated = true;

            var cookie = request.Cookies.GetCookie(".auth-IRunes");
            var cookieContent = cookie.Value;
            var userName = this.UserCookieService.GetUserData(cookieContent);
            return userName;
        }
    }
}
