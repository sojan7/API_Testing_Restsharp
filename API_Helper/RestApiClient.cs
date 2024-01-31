using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace API_Helper
{
    public class RestApiClient
    {
        private readonly RestClient client;
        private readonly RestRequest request;

        public RestApiClient(string baseUrl)
        {
            client = new RestClient(baseUrl);
            request = new RestRequest();
        }

        public RestApiClient AddQueryParameter(string name, string value)
        {
            request.AddQueryParameter(name, value);
            return this;
        }

        public RestApiClient AddUrlSegment(string name, string value)
        {
            request.AddUrlSegment(name, value);
            return this;
        }

        public RestApiClient AddJsonBody(object body)
        {
            request.AddJsonBody(body);
            return this;
        }

        public RestApiClient WithBasicAuthentication(HttpBasicAuthenticator httpBasicAuthenticator)
        {
            request.Authenticator = httpBasicAuthenticator;
            return this;
        }

        public RestApiClient WithCookieAuth(CookieCollection cookieCollection, string neededCookieName)
        {
            Cookie? cook = null;
            foreach (var cookie in cookieCollection.Where(cookie => cookie.Name == neededCookieName))
            {
                cook = cookie;
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

        public RestApiClient AddHeader(string name, string value)
        {
            request.AddHeader(name, value);
            return this;
        }

        public RestResponse Execute()
        {
            return client.Execute(request);
        }

        public RestResponse<T> Execute<T>()
        {
            return client.Execute<T>(request);
        }

        #region HTTP Methods

        public RestResponse<T> Get<T>(string resource, string? payload = null)
        {
            request.Method = Method.Get;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return ExecuteApi<T>();
        }

        public RestResponse<T> Post<T>(string resource, string? payload = null)
        {
            request.Method = Method.Post;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return ExecuteApi<T>();
        }

        public RestResponse<T> Put<T>(string resource, string? payload = null)
        {
            request.Method = Method.Put;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return ExecuteApi<T>();
        }

        public RestResponse<T> Patch<T>(string resource, string? payload = null)
        {
            request.Method = Method.Patch;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return ExecuteApi<T>();
        }

        public RestResponse<T> Delete<T>(string resource, string? payload = null)
        {
            request.Method = Method.Delete;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return ExecuteApi<T>();
        }

        private RestResponse<T> ExecuteApi<T>()
        {
            return client.Execute<T>(request);
        }

        #endregion HTTP Methods
    }
}