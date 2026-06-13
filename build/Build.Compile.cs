using Fallout.Common;
using Fallout.Common.Tools.DotNet;
using Fallout.Solutions;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using static Fallout.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    Target Compile => _ => _
    .TriggeredBy(Clean)
    .Executes(() =>
    {       
        foreach (var configuration in Solution.GetModel().BuildTypes)
        {
            Log.Information("Configuration name: {configuration}", configuration);

            if (configuration.StartsWith("Release"))
            {
                DotNetBuild(settings => settings
                    .SetConfiguration(configuration)
                    .SetVerbosity(DotNetVerbosity.quiet));
            }
        }

    });
}
