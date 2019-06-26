using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequest.Core
{
    /// <summary>
    /// HTTP-Requests
    /// </summary>
    public interface IHttpRequests
    {
        /// <summary>
        /// GET-Request
        /// </summary>
        Task<T> GetAsync<T>(string url) where T : class, new();
        /// <summary>
        /// GET-Requests
        /// with additianal headers
        /// </summary>
        Task<T> GetAsync<T>(string url, Dictionary<string, string> requestHeaders) where T : class, new();
        /// <summary>
        /// GET-Requests
        /// write json of obj to Body Stream
        /// </summary>
        Task<T> GetAsync<T>(string url, object obj) where T : class, new();


        /// <summary>
        /// PUT-Requests
        /// </summary>
        Task<T> PutAsync<T>(string url, object obj) where T : class, new();
        /// <summary>
        /// PUT-Requests
        /// with additianal headers
        /// </summary>
        Task<T> PutAsync<T>(string url, object obj, Dictionary<string, string> requestHeaders) where T : class, new();

        /// <summary>
        /// POST-Requests
        /// </summary>
        Task<T> PostAsync<T>(string url, object obj) where T : class, new();
        /// <summary>
        /// POST-Requests
        /// with additianal headers
        /// </summary>
        Task<T> PostAsync<T>(string url, object obj, Dictionary<string, string> requestHeaders) where T : class, new();


        /// <summary>
        /// DELETE-Requests
        /// </summary>
        Task<T> DeleteAsync<T>(string url) where T : class, new();
        /// <summary>
        /// DELETE-Requests
        /// with additianal headers
        /// </summary>
        Task<T> DeleteAsync<T>(string url, Dictionary<string, string> requestHeaders) where T : class, new();

        /// <summary>
        /// GET-Request 
        /// loading body stream as byte array
        /// </summary>
        Task<byte[]> DownloadFile(string url);
    }
}
