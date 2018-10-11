namespace HTTP.Responses.Contracts
{
    using Cookies.Contracts;
    using Headers;
    using Headers.Contracts;
    using HTTP.Cookies;
    using System.Net;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);

        byte[] GetBytes();
    }
}
