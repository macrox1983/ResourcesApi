using Microsoft.Extensions.DependencyInjection;
using Resources.Common;
using Resources.Common.Abstractions;
using Resources.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.Service.DI
{
    public static class ServiceCollectionResourcesServiceExtensions
    {
        public static IServiceCollection AddResourcesService(this IServiceCollection services)
        {
            services.AddSingleton<ResourcesDataContext>()
                .AddSingleton<IResourcesDataContext>( serviceProvider => 
                {
                    var dataContext = serviceProvider.GetService<ResourcesDataContext>();
                    dataContext.InitialDataContext().Wait();
                    return dataContext;
                })
                .AddTransient<IResourcesRepository, ResourcesRepository>()
                .AddTransient<IResourceService, ResourcesService>();

            return services;
        }
    }
}
