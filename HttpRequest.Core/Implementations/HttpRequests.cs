using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequest.Core
{
    /// <summary>
    /// HTTP-Requests
    /// </summary>
    public class HttpRequests: IHttpRequests, IDisposable
    {
        private HttpClient Client;

        /// <summary>
        /// Create new instance wrapped HttpClient
        /// </summary>
        /// <param name="configure">configure of HttpClient on create</param>
        public HttpRequests(Action<HttpClient> configure)
        {
            Client = new HttpClient();
            configure(Client);
        }

        /// <summary>
        /// GET-Request
        /// </summary>
        public async Task<T> GetAsync<T>(string url) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Get, url);
        }

        /// <summary>
        /// GET-Request
        /// write json of obj to Body Stream
        /// </summary>
        public async Task<T> GetAsync<T>(string url, object obj) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Get, url, obj);
        }

        /// <summary>
        /// GET-Request
        /// with additianal headers
        /// </summary>
        public async Task<T> GetAsync<T>(string url, Dictionary<string,string> requestHeaders) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Get, url, null, requestHeaders);
        }

        /// <summary>
        /// POST-Request
        /// </summary>
        public async Task<T> PostAsync<T>(string url, object obj) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Post, url, obj);
        }

        /// <summary>
        /// POST-Request
        /// with additianal headers
        /// </summary>
        public async Task<T> PostAsync<T>(string url, object obj , Dictionary<string, string> requestHeaders) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Post, url, obj, requestHeaders);
        }

        /// <summary>
        /// PUT-Request
        /// </summary>
        public async Task<T> PutAsync<T>(string url, object obj) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Put, url, obj);
        }

        /// <summary>
        /// PUT-Request
        /// with additianal headers
        /// </summary>
        public async Task<T> PutAsync<T>(string url, object obj, Dictionary<string, string> requestHeaders) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Put, url, obj, requestHeaders);
        }

        /// <summary>
        /// DELETE-Request
        /// </summary>
        public async Task<T> DeleteAsync<T>(string url) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Delete, url);
        }

        /// <summary>
        /// DELETE-Request
        /// with additianal headers
        /// </summary>
        public async Task<T> DeleteAsync<T>(string url, Dictionary<string, string> requestHeaders) where T : class, new()
        {
            return await MethodAsync<T>(HttpMethod.Delete, url, requestHeaders);
        }

        /// <summary>
        /// GET-Request
        /// loading body stream as byte array
        /// </summary>
        public async Task<byte[]> DownloadFile(string url)
        {
            return await MethodAsync<byte[]>(HttpMethod.Get, url);
        }

        /// <summary>
        /// Dispose HttpClient
        /// </summary>
        public void Dispose()
        {
            Client.Dispose();
        }


        private async Task<T> MethodAsync<T>(HttpMethod method, string url, object obj = null, Dictionary<string, string> requestHeaders = null) 
        {
            var uri = new Uri(url);

            using (var request = new HttpRequestMessage(method, uri))
            {
                if (obj != null)
                {
                    request.Content = GetStringContent(obj); 
                }

                if (requestHeaders != null)
                {
                    foreach (var requestHeader in requestHeaders)
                    {
                        request.Headers.Add(requestHeader.Key, requestHeader.Value);
                    }
                }
                

                using (var response = await Client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"[{method}]{url}. StatusCode: {(int)response.StatusCode}. {response.ReasonPhrase}");
                    }

                    if (typeof(T) == typeof(byte[]))
                    {
                        var bytes = await response.Content.ReadAsByteArrayAsync();
                        return (T)(bytes as object);
                    }

                    var responseString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseString);
                    }
                    catch (Exception)
                    {
                        throw new Exception($"[{method}]{url}. Response is not implement object '{typeof(T).Name}'");
                    }
                }
            }
        }


        /// <summary>
        /// Tranform to StringContent
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private StringContent GetStringContent(object obj)
        {
            StringContent content = null;
            if (obj != null)
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            return content;
        }
    }
}
