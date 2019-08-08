using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.WebHookService.DI
{
    public static class ServiceCollectionWebHookServiceExtensions
    {
        public static IServiceCollection AddWebHookService(this IServiceCollection services)
        {
            services.AddHostedService<WebHookService>();

            return services;
        }
    }
}
