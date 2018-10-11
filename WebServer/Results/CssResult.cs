namespace WebServer.Results
{
    using HTTP.Headers;
    using HTTP.Responses;
    using System.Net;
    using System.Text;

    public class CssResult : HttpResponse
    {
        public CssResult(string content, HttpStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/css"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
