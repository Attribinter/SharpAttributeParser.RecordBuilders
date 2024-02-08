namespace SharpAttributeParser.RecordBuilders;

using Microsoft.Extensions.DependencyInjection;

using System;

/// <summary>Allows the services of <i>SharpAttributeParser.RecordBuilders</i> to be registered with a <see cref="IServiceCollection"/>.</summary>
public static class SharpAttributeParserRecordBuildersServices
{
    /// <summary>Registers the services of <i>SharpAttributeParser.RecordBuilders</i> with the provided <see cref="IServiceCollection"/>.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> with which services are registered.</param>
    /// <returns>The provided <see cref="IServiceCollection"/>, so that calls can be chained.</returns>
    public static IServiceCollection AddSharpAttributeParserRecordBuilders(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<IRecorderWrapper, RecorderWrapper>();
        services.AddSingleton<ISemanticRecorderWrapper, SemanticRecorderWrapper>();
        services.AddSingleton<ISyntacticRecorderWrapper, SyntacticRecorderWrapper>();

        return services;
    }
}
