# netcore-httpRequests
.Net Core Libruary for simple call http-requests

#### Adding DI-service

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpRequests();
}

// or it's the same
public void ConfigureServices(IServiceCollection services)
{
    // It's .net core recomendation using single instance for  HttClient (created inside HttpRequests class)
    services.AddSingleton<IHttpRequests, HttpRequests>(provider => {
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
```

### example

```csharp
// steps:
// stringify `obj` to json-text
// write json-text to body stream
// send POST-request to `url`
// get response stream as text
// deserialize text to `MyClass`
HttpRequests = serviceProvider.GetService<IHttpRequests>(); 
await HttpRequests.PostAsync<MyClass>(url, obj);
```

