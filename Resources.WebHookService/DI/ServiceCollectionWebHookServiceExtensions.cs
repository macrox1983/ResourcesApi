using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resources.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.WebHookService.DI
{
    public static class ServiceCollectionWebHookServiceExtensions
    {
        public static IServiceCollection AddWebHookService(this IServiceCollection services)
        {
            services.AddSingleton<WebHookService>()
                .AddTransient<IHostedService>(serviceProvider => serviceProvider.GetService<WebHookService>());                

            return services;
        }
    }
}
