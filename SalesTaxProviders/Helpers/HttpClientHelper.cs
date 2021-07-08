using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using TaxProviders.Exceptions;

namespace TaxProviders.Helpers
{
    public class HttpClientHelper<TResult>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public HttpClientHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
        }

        private void AddBearerToken(string bearerToken)
        {
            if (!String.IsNullOrEmpty(bearerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", $"Bearer {bearerToken}");
            }
        }

        public async Task<TResult> GetAsync(string requestUri, Dictionary<string, string> queryParams, string bearerToken = null)
        {
            if(!String.IsNullOrEmpty(bearerToken))
                AddBearerToken(bearerToken);

            var url = _baseUrl + QueryHelpers.AddQueryString(requestUri, queryParams);

            using var httpResponse = await _httpClient.GetAsync(url);
            string response = await httpResponse.Content.ReadAsStringAsync();

            if ((int)httpResponse.StatusCode < 200 || (int)httpResponse.StatusCode >= 300)
            {
                throw new BadResponseException("Fail response", httpResponse.StatusCode, response);
            }
            return JsonConvert.DeserializeObject<TResult>(response, JsonSerializerSettings());
        }

        public async Task<TResult> PostAsync(string requestUri, StringContent content, string bearerToken = null)
        {
            if (!String.IsNullOrEmpty(bearerToken))
                AddBearerToken(bearerToken);

            var url = _baseUrl + requestUri;
            using var httpResponse = await _httpClient.PostAsync(url, content);
            httpResponse.EnsureSuccessStatusCode();
            string response = await httpResponse.Content.ReadAsStringAsync();

            if ((int)httpResponse.StatusCode < 200 || (int)httpResponse.StatusCode >= 300)
            {
                throw new BadResponseException("Fail response", httpResponse.StatusCode, response);
            }
            return JsonConvert.DeserializeObject<TResult>(response, JsonSerializerSettings());
        }

        private static JsonSerializerSettings JsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }
    }
}
