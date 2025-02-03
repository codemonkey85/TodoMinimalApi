using Todo.Api.Endpoints;

namespace Todo.Api.Middleware;

public static class WebApplicationEndpointMiddleware
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapTodoEndpoints();
        app.MapWeatherForecastEndpoints();
        return app;
    }    
}
