﻿using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace API_Helper
{
    public class RestApiClient : IDisposable
    {
        private readonly RestClient client;
        private readonly RestRequest request;

        public RestApiClient(string baseUrl)
        {
            client = new RestClient(baseUrl);
            request = new RestRequest();
        }

        public void Dispose()
        {
            client?.Dispose();
            GC.SuppressFinalize(this);
        }

        public RestApiClient WithQueryParameter(string name, string value)
        {
            request.AddQueryParameter(name, value);
            return this;
        }

        public RestApiClient WithUrlSegment(string name, string value)
        {
            request.AddUrlSegment(name, value);
            return this;
        }

        public RestApiClient WithJsonBody(object body)
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

        public RestApiClient WithHeader(string name, string value)
        {
            request.AddHeader(name, value);
            return this;
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

            return Execute<T>();
        }

        public T Get<T>(string resource, HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK, string? payload = null)
        {
            request.Method = Method.Get;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }
            var response = Execute<T>();

            if (response.StatusCode != expectedHttpStatusCode)
            {
                throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
            }

            return response.Data!;
        }

        public RestResponse<T> Post<T>(string resource, string? payload = null)
        {
            request.Method = Method.Post;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return Execute<T>();
        }

        public T Post<T>(string resource, HttpStatusCode expectedHttpStatusCode = HttpStatusCode.Created, string? payload = null)
        {
            request.Method = Method.Post;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }
            var response = Execute<T>();

            if (response.StatusCode != expectedHttpStatusCode)
            {
                throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
            }

            return response.Data!;
        }

        public RestResponse<T> Put<T>(string resource, string? payload = null)
        {
            request.Method = Method.Put;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return Execute<T>();
        }

        public T Put<T>(string resource, HttpStatusCode expectedHttpStatusCode, string? payload = null)
        {
            request.Method = Method.Put;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }
            var response = Execute<T>();

            if (response.StatusCode != expectedHttpStatusCode)
            {
                throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
            }

            return response.Data!;
        }

        public RestResponse<T> Patch<T>(string resource, string? payload = null)
        {
            request.Method = Method.Patch;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return Execute<T>();
        }

        public T Patch<T>(string resource, HttpStatusCode expectedHttpStatusCode, string? payload = null)
        {
            request.Method = Method.Patch;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }
            var response = Execute<T>();

            if (response.StatusCode != expectedHttpStatusCode)
            {
                throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
            }

            return response.Data!;
        }

        public RestResponse<T> Delete<T>(string resource, string? payload = null)
        {
            request.Method = Method.Delete;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return Execute<T>();
        }

        public T Delete<T>(string resource, HttpStatusCode expectedHttpStatusCode, string? payload = null)
        {
            request.Method = Method.Delete;
            request.Resource = resource;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }
            var response = Execute<T>();

            if (response.StatusCode != expectedHttpStatusCode)
            {
                throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
            }

            return response.Data!;
        }

        public RestResponse Delete(string resource, HttpStatusCode expectedHttpStatusCode)
        {
            request.Method = Method.Delete;
            request.Resource = resource;
            var response = Execute();
            if (response.StatusCode != expectedHttpStatusCode)
            {
                throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
            }
            return response;
        }

        private RestResponse<T> Execute<T>()
        {
            return client.Execute<T>(request);
        }

        private RestResponse Execute()
        {
            return client.Execute(request);
        }

        #endregion HTTP Methods
    }
}