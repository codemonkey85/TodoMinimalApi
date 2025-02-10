using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Serilog;
using Serilog.Core;
using Todo.Client;

try
{
    var logLevelSwitch = new LoggingLevelSwitch();
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.ControlledBy(logLevelSwitch)
        .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
        .WriteTo.BrowserHttp(endpointUrl: $"{builder.HostEnvironment.BaseAddress}ingest",
                             controlLevelSwitch: logLevelSwitch)
        .CreateLogger();

    builder.Logging.SetMinimumLevel(LogLevel.None);
    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddHttpClient("Todo.Api", client => client.BaseAddress = new Uri(
        builder.HostEnvironment.BaseAddress));

    builder.Services.AddMudServices(config =>
    {
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;

        config.SnackbarConfiguration.PreventDuplicates = true;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    });

    //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "WASM host crashed! {exMessage}", ex.Message);
}
