using Microsoft.Extensions.DependencyInjection;
using Resources.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.MessageBus.DI
{
    public static class ServiceCollectionMessageBusExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();

            return services;
        }
    }
}
