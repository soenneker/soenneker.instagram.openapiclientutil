using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Instagram.HttpClients.Abstract;

namespace Soenneker.Instagram.OpenApiClientUtil.Tests;

public sealed class AuthenticationTests
{
    [Test]
    public async ValueTask UsesConfiguredTokenBaseUrlAndSingleton(CancellationToken cancellationToken)
    {
        using var http = new HttpClient(new Handler());
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Instagram:AccessToken"] = "test-token",
            ["Instagram:ClientBaseUrl"] = "https://graph.facebook.com/v25.0"
        }).Build();
        await using var utility = new InstagramOpenApiClientUtil(new HttpProvider(http), configuration);
        var client = await utility.Get(cancellationToken: cancellationToken);
        if (!ReferenceEquals(client, await utility.Get(cancellationToken: cancellationToken))) throw new InvalidOperationException("Client was not reused");
        var result = await client["123"].GetAsync(cancellationToken: cancellationToken);
        if (result?.Id != "123") throw new InvalidOperationException("Response was not deserialized");
    }

    private sealed class HttpProvider(HttpClient client) : IInstagramOpenApiHttpClient
    {
        public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default) => ValueTask.FromResult(client);
        public void Dispose() { }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class Handler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri?.ToString() != "https://graph.facebook.com/v25.0/123") throw new InvalidOperationException("Configured base URL was ignored");
            if (request.Headers.Authorization?.ToString() != "Bearer test-token") throw new InvalidOperationException("Bearer token was not applied");
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"id\":\"123\"}", System.Text.Encoding.UTF8, "application/json") });
        }
    }
}