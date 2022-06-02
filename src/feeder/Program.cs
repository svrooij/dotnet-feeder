using CliFx;
using Feeder.Base;
using Feeder.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Feeder;

public static class Program
{
    public static async Task<int> Main()
    {
        var services = RegisterServices(new ServiceCollection());
        var provider = services.BuildServiceProvider();

        return await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(provider.GetService)
            .SetExecutableName("dotnet-feeder")
            .Build()
            .RunAsync();
    }

    private static ServiceCollection RegisterServices(ServiceCollection services)
    {
        services.AddHttpClient<IFeedClient, FeedClient>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IContentService, ContentService>();

        services.AddAllCommandsInAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    private static ServiceCollection AddAllCommandsInAssembly(this ServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes().Where(t => t.IsDefined(typeof(CliFx.Attributes.CommandAttribute)));
        foreach (var t in types)
        {
            services.AddTransient(t);
        }

        return services;
    }
}