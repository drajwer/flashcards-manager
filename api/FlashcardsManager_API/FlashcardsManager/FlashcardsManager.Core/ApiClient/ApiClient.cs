using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FlashcardsManager.Core.ApiClient
{
    public class ApiClient
    {
        private readonly ApiClientOptions _options;
        private readonly HttpClient _httpClient;
        public string BearerToken { get; set; }
        public ApiClient(HttpClient client)//IOptions<ApiClientOptions> optionsAccessor
        {
            _options = new ApiClientOptions
            {
                RequestsCount = 3,
                RequestsDelayInMilliseconds = 500
            }; //optionsAccessor.Value;
            _httpClient = client;
        }
        public async Task<TResponsebody> GetJsonAsync<TResponsebody>(string path)
        {
            return await SendRequestWithoutBody<TResponsebody>(path, HttpRequestType.Get, BearerToken);
        }

        public async Task<TResponsebody> PostJsonAsync<TRequestbody, TResponsebody>(string path, TRequestbody body)
        {
            return await SendRequestWithBody<TRequestbody, TResponsebody>(path, body, HttpRequestType.Post, BearerToken);
        }

        public async Task<bool> PostJsonAsync<TRequestbody>(string path, TRequestbody body)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(BearerToken))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            for (var i = 0; i < _options.RequestsCount; i++)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.PostAsync(path, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"TranslationManagerApiClientError: { ex.Message}");
                }
                Thread.Sleep(_options.RequestsDelayInMilliseconds);
                Console.WriteLine($"TranslationManagerApiClient: ({i + 1}/{_options.RequestsCount})");
            }
            return false;
        }

        public async Task<TResponsebody> PutJsonAsync<TRequestbody, TResponsebody>(string path, object id, TRequestbody body)
        {
            return await SendRequestWithBody<TRequestbody, TResponsebody>(path+"/"+id, body, HttpRequestType.Put, BearerToken);
        }

        public async Task<TResponsebody> DeleteJsonAsync<TResponsebody>(string path, object id)
        {
            return await SendRequestWithoutBody<TResponsebody>(path+"/"+id, HttpRequestType.Delete, BearerToken);
        }

        private async Task<TResponsebody> SendRequestWithBody<TRequestbody, TResponsebody>(string path, TRequestbody body, HttpRequestType type, string BearerToken = null)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(BearerToken))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            for (var i = 0; i < _options.RequestsCount; i++)
            {
                try
                {
                    HttpResponseMessage response;
                    switch (type)
                    {
                        case HttpRequestType.Post:
                            response = await _httpClient.PostAsync(path, content);
                            break;
                        case HttpRequestType.Put:
                            response = await _httpClient.PutAsync(path, content);
                            break;
                        default: throw new NotImplementedException();
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TResponsebody>(json);
                    }
                    Console.WriteLine($"TranslationManagerApiClientError: { response.StatusCode}");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"TranslationManagerApiClientError: { ex.Message}");
                }
                Thread.Sleep(_options.RequestsDelayInMilliseconds);
                Console.WriteLine($"TranslationManagerApiClient: ({i + 1}/{_options.RequestsCount})");
            }
            return default(TResponsebody);
        }

        private async Task<TResponsebody> SendRequestWithoutBody<TResponsebody>(string path, HttpRequestType type, string BearerToken = null)
        {
            if (!string.IsNullOrEmpty(BearerToken))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            for (int i = 0; i < _options.RequestsCount; i++)
            {
                try
                {
                    HttpResponseMessage response;
                    switch (type)
                    {
                        case HttpRequestType.Get:
                            response = await _httpClient.GetAsync(path);
                            break;
                        case HttpRequestType.Delete:
                            response = await _httpClient.DeleteAsync(path);
                            break;
                        default: throw new NotImplementedException();
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TResponsebody>(json);
                    }
                    Console.WriteLine($"TranslationManagerApiClientError: { response.StatusCode}");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"TranslationManagerApiClientError: { ex.Message}");
                }
                Thread.Sleep(_options.RequestsDelayInMilliseconds);
                Console.WriteLine($"TranslationManagerApiClient: ({i + 1}/{_options.RequestsCount})");
            }
            return default(TResponsebody);
        }
    }

    public enum HttpRequestType
    {
        Get = 0,
        Post = 1,
        Put = 2,
        Delete = 3
    }
}
