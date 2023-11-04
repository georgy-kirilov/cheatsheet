using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Shared.Api;

public static class SwaggerRegistration
{
    public static IServiceCollection AddAppSwagger<TProgram>(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = typeof(TProgram).Name,
                Version = "v1"
            });

            options.CustomSchemaIds(id => id.FullName?.Replace('+', '.'));
        });

        return services;
    }

    public static IApplicationBuilder UseAppSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        return app;
    }
}
