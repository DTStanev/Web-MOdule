namespace WebServer.Results
{
    using HTTP.Cookies;
    using HTTP.Headers;
    using HTTP.Responses;
    using System.Net;
    using System.Text;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.AddCookie(new HttpCookie("lang", "bg"));
            this.AddCookie(new HttpCookie("somecookie", "123"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
