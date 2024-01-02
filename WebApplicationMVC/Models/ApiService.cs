﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplicationMVC.Models {
    public static class ApiService {

        public static int ResponseCode { get; private set; }

        public static async Task<T> Get<T>(String endpoint) {

            using(HttpClient client = new HttpClient()) {

                HttpResponseMessage response = await client.GetAsync(endpoint);

                if(response.IsSuccessStatusCode) {
                    string responseData = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(responseData);
                }
                else {
                    ResponseCode = (int)response.StatusCode;
                    return default;
                }
            }

        }

        public static async Task<T> Post<T>(String endpoint, T data) {

            string dataAsString = "";

            try {
                dataAsString = JsonConvert.SerializeObject(data);
            }
            catch (Exception ex) {
                throw;
            }

            using(HttpClient client = new HttpClient()) {

                HttpContent content = new StringContent(dataAsString, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                if(response.IsSuccessStatusCode) {
                    string responseData = await content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(responseData);
                }
                else {
                    ResponseCode = (int)response.StatusCode;
                    return default;
                }
            }
        }

    }
}
