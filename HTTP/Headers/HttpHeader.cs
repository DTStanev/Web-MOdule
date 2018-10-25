namespace HTTP.Headers
{
    using HTTP.Common;

    public class HttpHeader
    {
        public const string COOKIE = "Cookie";

        public const string CONTENT_TYPE = "Content-Type";

        public const string CONTENT_LENGTH = "Content-Length";

        public const string CONTENT_DISPOSITION = "Content-Disposition";

        public const string AUTHORIZATION = "Authorization";

        public const string HOST = "Host";

        public const string LOCATION = "Location";

        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Key = key;
            this.Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public override string ToString()
        {
            return $"{this.Key}: {this.Value}";
        }
    }
}
