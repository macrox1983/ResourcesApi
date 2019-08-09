using Microsoft.Extensions.DependencyInjection;
using Resources.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.PerfomanceMonitoring.DI
{
    public static class ServiceCollectionPerfomanceMonitorExtensions
    {
        public static IServiceCollection AddPerfomanceMonitor(this IServiceCollection services)
        {
            services.AddSingleton<IPerfomanceMonitor, PerfomanceMonitor>().AddSingleton<ITelemetryCollector, TelemetryCollector>();

            return services;
        }
    }
}
