using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Shared.Api;

public static class SwaggerRegistration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ModMonTemplate",
                Version = "v1"
            });

            options.CustomSchemaIds(id => id.FullName?.Replace('+', '.'));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.AddServer(new OpenApiServer
            {
                Url = "/backend",
                Description = "nginx"
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerInDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            SwaggerBuilderExtensions.UseSwagger(app);

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/backend/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        return app;
    }
}
