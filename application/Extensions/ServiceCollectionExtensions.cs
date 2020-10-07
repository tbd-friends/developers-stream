using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHandlers(this IServiceCollection collection)
        {
            collection.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
        }
    }
}