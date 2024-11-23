using Serilog;

namespace SelfCheckoutServiceMachine.Config;

public class LoggingConfig
{
    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }

}