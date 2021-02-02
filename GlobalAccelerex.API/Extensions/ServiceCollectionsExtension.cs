using GlobalAccelerex.API.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GlobalAccelerex.API.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static  class ServiceCollectionsExtension
    {
        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GA API",
                    Version = "v1",
                    License = new OpenApiLicense
                    {
                        Name = "Microsoft Licence",
                        Url = new Uri("https://google.com"),
                    },
                    Description = "Global Accelerex: Restaurant Opening Hour"
                });
                c.SchemaFilter<EnumSchemaFilter>();

                string basePath = PlatformServices.Default.Application.ApplicationBasePath;
                string fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                var location = Path.Combine(basePath, fileName);
                c.IncludeXmlComments(location);

            });
            services.AddSwaggerGenNewtonsoftSupport();
            return services;
        }

    }
}
