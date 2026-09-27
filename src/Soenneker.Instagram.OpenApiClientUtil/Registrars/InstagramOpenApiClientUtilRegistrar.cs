using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Instagram.HttpClients.Registrars;
using Soenneker.Instagram.OpenApiClientUtil.Abstract;

namespace Soenneker.Instagram.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class InstagramOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="InstagramOpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddInstagramOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddInstagramOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IInstagramOpenApiClientUtil, InstagramOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="InstagramOpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddInstagramOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddInstagramOpenApiHttpClientAsSingleton()
                .TryAddScoped<IInstagramOpenApiClientUtil, InstagramOpenApiClientUtil>();

        return services;
    }
}
