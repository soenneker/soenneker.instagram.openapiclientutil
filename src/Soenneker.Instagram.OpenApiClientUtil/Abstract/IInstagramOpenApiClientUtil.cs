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
    ValueTask<InstagramOpenApiClient> Get(CancellationToken cancellationToken = default);
}
