using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinFormsApp.Exceptions;

namespace WinFormsApp.Controllers
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _url = "https://localhost:7021/";
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _url + url);
            var response = await SendRequestAsync<T>(requestMessage);

            return response;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _url + url)
            {
                Content = httpContent
            };

            var response = await SendRequestAsync<TResponse>(requestMessage);

            return response;
        }

        public async Task DeleteAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, _url + url);
            await SendRequestAsync<object>(requestMessage);
        }

        private async Task<T> SendRequestAsync<T>(HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    throw new NoBookingException();
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    throw new OverBookingException(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new HttpRequestException();
                }
                
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse)!;
        }
    }
}
