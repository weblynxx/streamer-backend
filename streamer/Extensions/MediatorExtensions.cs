using System.Linq;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using streamer.Features.User.Login;

namespace streamer.Extensions
{
    public static class MediatorExtensions
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services, Assembly assembly)
        {
            
            var classTypes = typeof(LoginCommand).GetTypeInfo().Assembly.ExportedTypes.Select(t => t.GetTypeInfo()).Where(t => t.IsClass && !t.IsAbstract);
            Log.Information("Mediator-start");
            foreach (var type in classTypes)
            {
                var interfaces = type.ImplementedInterfaces.Select(i => i.GetTypeInfo());

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                {
                    Log.Information("Mediator-reg:"+ handlerType.AsType() + " "+type.AsType());
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }

                //foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>)))
                //{
                //    services.AddTransient(handlerType.AsType(), type.AsType());
                //}
            }

            
            
            return services;
        }
    }
}
