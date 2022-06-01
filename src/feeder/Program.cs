using CliFx;
using System;

namespace Feeder
{
    public static class Program
    {
        public static async Task<int> Main() =>
               await new CliApplicationBuilder()
                   .AddCommandsFromThisAssembly()
                   .SetExecutableName("dotnet-feeder")
                   .Build()
                   .RunAsync();
       
    }
}