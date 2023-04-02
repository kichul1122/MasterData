using Cysharp.Text;
using MasterData.Generator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;

//args = new string[] { "excutedirectory", "-i", "C:/Project/god/MasterData", "-o", "C:/Project/god/MasterData/cs", "-n", "GODII" };

var builder = ConsoleApp.CreateBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.AddOptions();

    services.AddLogging(logging =>
    {
        logging.ClearProviders();

        logging.SetMinimumLevel(LogLevel.Information);

        logging.AddZLoggerConsole(options =>
        {
            options.PrefixFormatter = (writer, info) =>
            {
                if (info.LogLevel == LogLevel.Error)
                {
                    ZString.Utf8Format(writer, "\u001b[31m[{0}]", info.LogLevel);
                }
            };
        }, configureEnableAnsiEscapeCode: true);

        logging.AddZLoggerRollingFile((dt, x) => $"logs/{nameof(Generator)}_{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log", x => x.ToLocalTime().Date, 1024);
    });
});

var app = builder.Build();

app.AddCommands<Generator>();
app.Run();