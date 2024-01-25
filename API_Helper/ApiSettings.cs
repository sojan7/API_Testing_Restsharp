using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace API_Helper
{
    public class ApiSettings
    {
        private readonly RestClient client;
        private readonly RestRequest request;
        private readonly string baseUrl;

        private string GetBaseUrl()
        {
            // Implement logic to fetch the base URL dynamically
            return "";
        }

        public ApiSettings(string resource, Method method, string? baseUrl = null)
        {
            this.baseUrl = baseUrl ?? GetBaseUrl();
            client = new RestClient(this.baseUrl);
            request = new RestRequest(resource, method);
        }

        public ApiSettings(string baseUrl)
        {
            this.baseUrl = baseUrl ?? GetBaseUrl();
            client = new RestClient(this.baseUrl);
            request = new RestRequest();
        }

        public ApiSettings AddQueryParameter(string name, string value)
        {
            request.AddQueryParameter(name, value);
            return this;
        }

        public ApiSettings AddUrlSegment(string name, string value)
        {
            request.AddUrlSegment(name, value);
            return this;
        }

        public ApiSettings AddJsonBody(object body)
        {
            request.AddJsonBody(body);
            return this;
        }

        public ApiSettings WithBasicAuthentication(HttpBasicAuthenticator httpBasicAuthenticator)
        {
            request.Authenticator = httpBasicAuthenticator;
            return this;
        }

        public ApiSettings WithCookieAuth(CookieCollection cookieCollection, string neededCookieName)
        {
            Cookie? cook = null;
            foreach (Cookie cookie in cookieCollection)
            {
                if (cookie.Name == neededCookieName)
                {
                    cook = cookie;
                }
            }

            if (cook is null)
            {
                request.AddCookie(string.Empty, string.Empty, string.Empty, string.Empty);
            }
            else
            {
                request.AddCookie(cook.Name, cook.Value, cook.Path, cook.Domain);
            }

            return this;
        }

        public ApiSettings AddHeader(string name, string value)
        {
            request.AddHeader(name, value);
            return this;
        }

        public RestRequest Build() => request;

        public RestResponse Execute()
        {
            return client.Execute(request);
        }

        public RestResponse<T> Execute<T>()
        {
            return client.Execute<T>(request);
        }

        #region HTTP Methods

        public static ApiSettings Get(string resource, string? baseUrl = null)
        {
            return new ApiSettings(resource, Method.Get, baseUrl!);
        }

        public static ApiSettings Post(string resource, string? baseUrl = null)
        {
            return new ApiSettings(resource, Method.Post, baseUrl!);
        }

        public static ApiSettings Put(string resource, string? baseUrl = null)
        {
            return new ApiSettings(resource, Method.Put, baseUrl!);
        }

        public static ApiSettings Patch(string resource, string? baseUrl = null)
        {
            return new ApiSettings(resource, Method.Patch, baseUrl!);
        }

        public static ApiSettings Delete(string resource, string? baseUrl = null)
        {
            return new ApiSettings(resource, Method.Delete, baseUrl!);
        }

        #endregion HTTP Methods
    }
}