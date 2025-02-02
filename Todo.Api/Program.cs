using AspNetCore.Swagger.Themes;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Todo.Api.Data;
using Todo.Api.Middleware;

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    IServiceCollection? services = builder.Services;

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration));

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

    if (builder.Environment.IsDevelopment())
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
    }

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi("/openapi/v1/openapi.json");
        app.UseSwaggerUI(ModernStyle.Dark, options =>
        {
            options.SwaggerEndpoint("/openapi/v1/openapi.json", "Todo API");
        });
    }

    app.UseHttpsRedirection();

    app.MapApiEndpoints();

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
