using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Unofficial.Owlet.EndpointClients
{
    public class AylaServiceClient
    {
        protected readonly HttpClient _httpClient;

        protected AylaServiceClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        protected async Task<HttpResponseMessage> SendRequest(HttpMethod method, string urlPath, HttpContent content = null, IDictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(method, $"{this._httpClient.BaseAddress}{urlPath}");
            if (headers != null && headers.Any())
            {
                foreach (var keyValuePair in headers)
                {
                    if (requestMessage.Headers.Contains(keyValuePair.Key))
                    {
                        requestMessage.Headers.Remove(keyValuePair.Key);
                    }
                    requestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            requestMessage.Content = content;
            return await this._httpClient.SendAsync(requestMessage);
        }

        protected HttpContent ToJsonPayload(object payload)
        {
            var data = JsonConvert.SerializeObject(payload);
            return new StringContent(data, Encoding.UTF8, "application/json");
        }

        protected async Task<T> GetFromResponse<T>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        protected void CheckArgument(string argument, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException($"No {argumentName} available");
            }
        }
        protected void CheckArgument(int argument, string argumentName)
        {
            if (argument == 0)
            {
                throw new ArgumentException($"No {argumentName} available");
            }
        }
    }

    public static class ServiceClientExtensions
    {
        public static IDictionary<string, string> AddAuthorizationHeader(this IDictionary<string, string> headers, string accessToken)
        {
            const string authorizationKey = "Authorization";

            if (headers.ContainsKey(authorizationKey))
            {
                headers.Remove(authorizationKey);
            }
            headers.Add(authorizationKey, $"auth_token {accessToken}");
            return headers;
        }
    }
}
