﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MovieSearch.Application.Ports;
using MovieSearch.Application.Services;

namespace MovieSearch.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.Decorate<ISearchMovie, SearchRequestLogDecorator>();
        
        return services;
    }
}