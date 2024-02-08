namespace SharpAttributeParser.RecordBuilders.SharpAttributeParserRecordBuildersServicesCases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Moq;

using System;

using Xunit;

public sealed class AddSharpAttributeParserRecordBuilders
{
    private static IServiceCollection Target(IServiceCollection services) => SharpAttributeParserRecordBuildersServices.AddSharpAttributeParserRecordBuilders(services);

    private readonly IServiceProvider ServiceProvider;

    public AddSharpAttributeParserRecordBuilders()
    {
        HostBuilder host = new();

        host.ConfigureServices(static (services) => Target(services));

        ServiceProvider = host.Build().Services;
    }

    [Fact]
    public void NullServiceCollection_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void ValidServiceCollection_ReturnsSameServiceCollection()
    {
        var serviceCollection = Mock.Of<IServiceCollection>();

        var actual = Target(serviceCollection);

        Assert.Same(serviceCollection, actual);
    }

    [Fact]
    public void IRecorderWrapper_ServiceCanBeResolved() => ServiceCanBeResolved<IRecorderWrapper>();

    [Fact]
    public void ISemanticRecorderWrapper_ServiceCanBeResolved() => ServiceCanBeResolved<ISemanticRecorderWrapper>();

    [Fact]
    public void ISyntacticRecorderWrapper_ServiceCanBeResolved() => ServiceCanBeResolved<ISyntacticRecorderWrapper>();

    [AssertionMethod]
    private void ServiceCanBeResolved<TService>() where TService : notnull
    {
        var service = ServiceProvider.GetRequiredService<TService>();

        Assert.NotNull(service);
    }
}
