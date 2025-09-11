using POS_display.Properties;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;

namespace POS_display.Utils.Logging
{
    public static class Serilogger
    {
        private static ILogger Logger;
        private static LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();
        public static ILogger GetLogger(string logIndex = "")
        {
            if (Logger != null)
                return Logger;

            if (Session.getParam("LOGGING", "ON").Equals("0") || Session.Develop)
                levelSwitch.MinimumLevel = (LogEventLevel)1 + (int)LogEventLevel.Fatal;

            if (string.IsNullOrEmpty(logIndex))
                logIndex = Session.Develop ? $"{Settings.Default.DefaultLogIndex}-test": Settings.Default.DefaultLogIndex;

            Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                    new Uri($"https://{Settings.Default.ElkUser}:" +
                                    $"{Settings.Default.ElkPassword}@" +
                                    $"{Settings.Default.ElkIP}" +
                                    $":{Settings.Default.ElkPort}/"))
                {
                    ModifyConnectionSettings = configuration => configuration.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => true),
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{logIndex}-{DateTime.UtcNow:yyyy-MM}",
                    PipelineName = "json"
                })
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .CreateLogger();

            return Logger;
        }
    }
}

