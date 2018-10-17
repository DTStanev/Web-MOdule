using HTTP.Headers;
using HTTP.Requests.Contracts;
using HTTP.Responses;
using HTTP.Responses.Contracts;
using MvcFramework.Services;
using MvcFramework.Services.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MvcFramework
{
    public abstract class Controller
    {
        protected const string VIEWS_FOLDER_PATH = "../../../Views/";

        protected const string HTML_EXTENTION = ".html";      
        
        private const string CONTROLLER_WORD = "Controller";

        private const string LAYOUT = "_Layout";

        private const string SLASH_SYMBOL = "/";

        private const string AUTH_COOKIE_KEY = ".auth-IRunes";
        
        protected Controller()
        {         
            this.ViewBag = new Dictionary<string, string>();
            this.Response = new HttpResponse { StatusCode = HttpStatusCode.OK };
        }

        protected Dictionary<string, string> ViewBag { get; set; }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        public IUserCookieService UserCookieService { get; internal set; }

        public string User
        {
            get
            {
                if (!this.Request.Cookies.ContainsCookie(AUTH_COOKIE_KEY))
                {
                    return null;
                }

                var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);
                return userName;
            }
            
        }

        private string GetUsername()
        {
            if (!this.Request.Cookies.ContainsCookie(AUTH_COOKIE_KEY))
            {
                return null;
            }
            var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
            var cookieContent = cookie.Value;
            var userName = this.UserCookieService.GetUserData(cookieContent);
            return userName;
        }

        protected IHttpResponse View(string viewName)
        {
            var layout = System.IO.File.ReadAllText(VIEWS_FOLDER_PATH + LAYOUT + HTML_EXTENTION);
            var contentFolderName = this.GetType().Name.Replace(CONTROLLER_WORD, SLASH_SYMBOL);
            var content = System.IO.File.ReadAllText(VIEWS_FOLDER_PATH + contentFolderName + viewName + HTML_EXTENTION);
            this.SetViewBagParameters(content);

            var allcontent = this.GetFullViewContent(layout);
            this.PrepareHtmlResult(allcontent);
            return this.Response;
        }

        //protected IHttpResponse BadRequestError(string errorMessage)
        //{
        //    var viewBag = new Dictionary<string, string>();
        //    viewBag.Add("Error", errorMessage);
        //    var allContent = this.GetViewContent("Error", viewBag);
        //    return new HtmlResult(allContent, HttpStatusCode.BadRequest);
        //}

        //protected IHttpResponse ServerError(string errorMessage)
        //{
        //    var viewBag = new Dictionary<string, string>();
        //    viewBag.Add("Error", errorMessage);
        //    var allContent = this.GetViewContent("Error", viewBag);
        //    return new HtmlResult(allContent, HttpStatusCode.InternalServerError);
        //}

            protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Response.Content = content;
            return this.Response;
        }
         protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.Location, location));
            this.Response.StatusCode = HttpStatusCode.SeeOther;
            return this.Response;
        }
         protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }	        

        private void PrepareHtmlResult(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
        }


        protected void SetViewBagParameters(string content)
        {
            var bodyKey = "@RenderBody()";
            var nonAuthenticatedKey = "@IsNotAuthenticated";
            var authenticatedKey = "@IsAuthenticated";

            this.ViewBag[bodyKey] = content;
            this.ViewBag[nonAuthenticatedKey] = "";
            this.ViewBag[authenticatedKey] = "";

            if (this.User == null)
            {
                this.ViewBag[authenticatedKey] = "none";
            }
            else
            {
                this.ViewBag[nonAuthenticatedKey] = "none";
            }
        }

        private string GetFullViewContent(string content)
        {
            content = content.Replace("@RenderBody()", ViewBag["@RenderBody()"]);

            foreach (var viewbagKey in this.ViewBag.Keys)
            {
                //TODO: Can we miss contains check?
                if (content.Contains(viewbagKey))
                {
                    content = content.Replace(viewbagKey, this.ViewBag[viewbagKey]);
                }
            }

            return content;
        }

    }
}
