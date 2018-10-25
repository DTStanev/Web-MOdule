namespace MvcFramework
{
    using HTTP.Headers;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using MvcFramework.ErrorViewModels;
    using Services.Contracts;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using ViewEngine.Contracts;

    public abstract class Controller
    {
        protected const string VIEWS_FOLDER_PATH = "../../../Views/";
        protected const string HTML_EXTENTION = ".html";
        protected const string AUTH_COOKIE_KEY = ".auth-IRunes";
        private const string CONTROLLER_WORD = "Controller";
        private const string LAYOUT = "_Layout";
        private const string ERROR_VIEW_PATH = "Error/Error";
        private const string ERROR_VIEW_NAME = "Error";
        private const string BODY_PLACEHOLDER = "@RenderBody()";       
        private const string SLASH_SYMBOL = "/";

        protected Controller()
        {
            this.Response = new HttpResponse { StatusCode = HttpStatusCode.OK };
        }

        private string GetCurrentControllerName => this.GetType().Name.Replace(CONTROLLER_WORD, string.Empty);

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        public IUserCookieService UserCookieService { get; internal set; }

        public IViewEngine ViewEngine { get; set; }       

        protected string User
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
        protected IHttpResponse View(string viewName)
        {
            var allContent = this.GetViewContent(viewName, (object)null);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse View<T>(string viewName, T model = null)
            where T : class
        {
            var allContent = this.GetViewContent(viewName, model);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_LENGTH, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_DISPOSITION, "inline"));
            this.Response.Content = content;
            return this.Response;
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.LOCATION, location));
            this.Response.StatusCode = HttpStatusCode.SeeOther;
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_TYPE, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }

        protected IHttpResponse BadRequestError<T>(string viewName, string errorMessage, T model = null)
            where T: class
        {
            var errorViewModel = new ErrorViewModel { Message = errorMessage };
            var allContent = this.GetViewContent(viewName, model,  errorViewModel);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpStatusCode.BadRequest;
            return this.Response;
        }

        protected IHttpResponse BadRequestError(string viewName, string errorMessage)
            
        {
            var errorViewModel = new ErrorViewModel { Message = errorMessage }  ;
            var allContent = this.GetViewContent(viewName, errorViewModel);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpStatusCode.BadRequest;
            return this.Response;
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var viewModel = new ErrorViewModel { Message = errorMessage };
            //TODO: Making view for internal server error 
            var allContent = this.GetViewContent(ERROR_VIEW_NAME, viewModel);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpStatusCode.InternalServerError;
            return this.Response;
        }

        private string GetViewContent<T1>(string viewName, T1 model, ErrorViewModel errorViewModel = null)
            where T1: class
        {          
            string errorContent = string.Empty;

            if (errorViewModel != null || model is ErrorViewModel)
            {
                var errorHtml = this.GetFileContent(ERROR_VIEW_NAME);

                if (model is ErrorViewModel)
                {
                    errorContent = this.ViewEngine.GetHtml(ERROR_VIEW_NAME, errorHtml, model, this.User); 
                }
                else
                {
                    errorContent = this.ViewEngine.GetHtml(ERROR_VIEW_NAME, errorHtml, errorViewModel, this.User);
                }
            }

            var viewFileContent = this.GetFileContent(viewName);
            var content = this.ViewEngine.GetHtml(viewName, viewFileContent, model, this.User);
            content = string.Concat(errorContent, content);

            var layoutFileContent = this.GetFileContent(LAYOUT);
            var allContent = layoutFileContent.Replace(BODY_PLACEHOLDER, content);
            var layoutContent = this.ViewEngine.GetHtml(LAYOUT, allContent, model, this.User);
            return layoutContent;
        }

        private string GetFileContent(string viewName)
        {
            string fullPath = null;

            switch (viewName)
            {
                case ERROR_VIEW_NAME:
                    fullPath = $"{VIEWS_FOLDER_PATH}{ERROR_VIEW_PATH}{HTML_EXTENTION}";
                    break;
                case LAYOUT:
                    fullPath = $"{VIEWS_FOLDER_PATH}{LAYOUT}{HTML_EXTENTION}";
                    break;
                default:
                    var contentFolderName = this.GetType().Name.Replace(CONTROLLER_WORD, SLASH_SYMBOL);
                    fullPath = $"{VIEWS_FOLDER_PATH}{contentFolderName}{viewName}{HTML_EXTENTION}";
                    break;
            }

            return System.IO.File.ReadAllText(fullPath);
        }

        private void PrepareHtmlResult(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_TYPE, "text/html; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
