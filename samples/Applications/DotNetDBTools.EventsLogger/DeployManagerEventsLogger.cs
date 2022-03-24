using System.Reflection;
using DotNetDBTools.Deploy;
using Serilog;
using Serilog.Events;

namespace DotNetDBTools.EventsLogger
{
    public static class DeployManagerEventsLogger
    {
        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        static DeployManagerEventsLogger()
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File($"{s_samplesOutputDir}/logs/{assemblyName}/logInfo.txt",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    fileSizeLimitBytes: 50_000,
                    rollOnFileSizeLimit: true)
                .WriteTo.File($"{s_samplesOutputDir}/logs/{assemblyName}/logDebug.txt",
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    fileSizeLimitBytes: 250_000,
                    rollOnFileSizeLimit: true)
                .WriteTo.File($"{s_samplesOutputDir}/logs/{assemblyName}/logVerbose.txt",
                    restrictedToMinimumLevel: LogEventLevel.Verbose,
                    fileSizeLimitBytes: 2_500_000,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
        }

        public static void LogEvent(EventFiredEventArgs eventArgs)
        {
            LogInfo(eventArgs);
            LogDebug(eventArgs);
            LogVerbose(eventArgs);
        }

        private static void LogInfo(EventFiredEventArgs eventArgs)
        {
            string msg = eventArgs.EventType switch
            {
                EventType.RegisterBegan => "Registering database...",
                EventType.UnregisterBegan => "Unregistering database...",
                EventType.PublishBegan => "Publishing database...",
                EventType.GeneratePublishScriptBegan => "Generating publish script...",
                EventType.GenerateDefinitionBegan => "Generating definition...",
                EventType.CreateDbModelFromDefinitionBegan => "Creating db model from definition...",
                EventType.CreateDbModelFromDBMSBegan => "Creating db model from DBMS...",
                EventType.CreateDbDiffBegan => "Creating db diff...",
                EventType.ApplyDbDiffBegan => "Applying db diff...",
                EventType.BeginTransactionFinished => "Transaction started",
                EventType.CommitTransactionFinished => "Transaction committed",
                EventType.RollbackTransactionFinished => "Transaction rolled back",
                _ => "",
            };

            if (msg == "")
                return;

            Log.Information(msg);
        }

        private static void LogDebug(EventFiredEventArgs eventArgs)
        {
            if (eventArgs.EventType is EventType.QueryBegan or EventType.QueryFinished)
                return;

            Log.Debug($"Event fired: '{eventArgs.EventType}'");
        }

        private static void LogVerbose(EventFiredEventArgs eventArgs)
        {
            if (eventArgs.EventData is not null)
                Log.Verbose($"Event '{eventArgs.EventType}' fired with data:\n{eventArgs.EventData}\n");
        }
    }
}
