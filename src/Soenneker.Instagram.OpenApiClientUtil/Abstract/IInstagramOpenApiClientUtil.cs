using Soenneker.Instagram.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Instagram.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface IInstagramOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the cached, authenticated Instagram Graph API client.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel client initialization.</param>
    /// <returns>The configured OpenAPI client.</returns>
    ValueTask<InstagramOpenApiClient> Get(CancellationToken cancellationToken = default);
}
