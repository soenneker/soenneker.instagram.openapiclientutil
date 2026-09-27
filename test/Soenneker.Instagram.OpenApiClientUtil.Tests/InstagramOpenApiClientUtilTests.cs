using Soenneker.Instagram.OpenApiClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Instagram.OpenApiClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class InstagramOpenApiClientUtilTests : HostedUnitTest
{
    private readonly IInstagramOpenApiClientUtil _openapiclientutil;

    public InstagramOpenApiClientUtilTests(Host host) : base(host)
    {
        _openapiclientutil = Resolve<IInstagramOpenApiClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
