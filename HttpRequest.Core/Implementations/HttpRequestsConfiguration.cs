using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace HttpRequest.Core
{
    public static class HttpRequestsConfiguration
    {
        /// <summary>
        /// Add HttpRequests to DI-services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpRequests(this IServiceCollection services)
        {
            // It's .net core recomendation using single instance for  HttClient (created inside HttpRequests class)
            services.AddSingleton<IHttpRequests, HttpRequests>(provider => {
                // connection per host
                // ServicePointManager.DefaultConnectionLimit = 5; // 2 by default

                var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

                return new HttpRequests(client => {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", appName);
                    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                });
            });

            return services;
        }
    }
}
