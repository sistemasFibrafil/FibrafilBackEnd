using Net.Data;
using System.Net.Http;
using Net.Connection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Net.Business.Services
{
    public static class ServiceExtensions
    {

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(opt => { opt.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://192.168.1.13", "http://localhost:80", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod()); });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        public static void ConfigureSQLConnection(this IServiceCollection services)
        {
            services.AddScoped<IConnectionSql, ConnectionSql>();
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            //services.AddSingleton<DriveApiService>();
        }

        public static void ConfigureHttpClientServiceLayer(this IServiceCollection services)
        {
            services.AddHttpClient("bypass-ssl-validation")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (
                    httpRequestMessage, cert, certChain, policyErrors) =>
                {
                    return true;
                }
            });
        }
    }
}
