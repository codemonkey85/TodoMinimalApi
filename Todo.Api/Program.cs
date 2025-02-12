using AspNetCore.Swagger.Themes;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using Todo.Api.Data;
using Todo.Api.Helpers;
using Todo.Api.Middleware;

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    IServiceCollection? services = builder.Services;

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration));

    services.AddProblemDetails();
    services.AddExceptionHandler<CustomExceptionHandler>();

    // Add services to the container.
    string connectionString = builder.Configuration.GetConnectionString("DataContext")
        ?? throw new InvalidOperationException("Unable to retrieve ConnectionString: `DataContext`");
    services.AddDbContext<DataContext>(options => 
    {
        options.UseSqlServer(connectionString);
        if (builder.Environment.IsDevelopment())
        {
            options.EnableDetailedErrors();
        }
    });

    // Fluent Validation
    services.AddValidators();
    // API / DB Services
    services.AddApiServices();

    services.AddAutoMapper(Assembly.GetExecutingAssembly());

    if (builder.Environment.IsDevelopment())
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();        
    }

    services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("https://localhost:5173");
        });
    });

    var app = builder.Build();
    using IServiceScope? scope = app.Services.CreateScope();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi("/openapi/v1/openapi.json");
        app.UseSwaggerUI(ModernStyle.Dark, options =>
        {
            options.SwaggerEndpoint("/openapi/v1/openapi.json", "Todo API");
        });
        app.UseWebAssemblyDebugging();

#if DEBUG
        bool seed = app.Configuration.GetValue<bool>("seed");
        if (seed)
        {            
            using DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
            DbInit.Seed(context);
        }
#endif
    }

    app.UseHttpsRedirection();

    app.UseSerilogIngestion();
    app.UseSerilogRequestLogging();

    app.UseBlazorFrameworkFiles();
    app.MapStaticAssets();
    
    app.MapApiEndpoints();

    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errors?view=aspnetcore-9.0#iproblemdetailsservice-fallback
    app.UseExceptionHandler();

    app.UseRouting();

    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception ex)
{
    if (ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
        throw;
    Log.Fatal(ex, "{exMessage}", ex.Message);
}
finally
{
    Log.CloseAndFlush();
}
