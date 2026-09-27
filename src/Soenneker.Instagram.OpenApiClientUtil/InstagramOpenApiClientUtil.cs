using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Instagram.HttpClients.Abstract;
using Soenneker.Instagram.OpenApiClientUtil.Abstract;
using Soenneker.Instagram.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Instagram.OpenApiClientUtil;
public sealed class InstagramOpenApiClientUtil : IInstagramOpenApiClientUtil
{
    private readonly AsyncSingleton<InstagramOpenApiClient> _client;

    public InstagramOpenApiClientUtil(IInstagramOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<InstagramOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("Instagram:AccessToken");
            string authHeaderName = configuration["Instagram:AuthHeaderName"] ?? "Authorization";
            string authHeaderValueTemplate = configuration["Instagram:AuthHeaderValueTemplate"] ?? "Bearer {token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var requestAdapter = new HttpClientRequestAdapter(new GenericAuthenticationProvider(headerName: authHeaderName, headerValue: authHeaderValue),
                httpClient: httpClient);
            string? baseUrl = configuration["Instagram:ClientBaseUrl"];
            if (!string.IsNullOrWhiteSpace(baseUrl)) requestAdapter.BaseUrl = baseUrl;

            return new InstagramOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<InstagramOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
