using Microsoft.Extensions.DependencyInjection;
using XdtPlayground.Monaco.Interop;

namespace XdtPlayground.Monaco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMonaco(this IServiceCollection services)
        {
            services.AddSingleton<MonacoInterop>();

            return services;
        }
    }
}
