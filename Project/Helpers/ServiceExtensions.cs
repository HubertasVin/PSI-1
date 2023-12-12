using Castle.DynamicProxy;
using Project.Interceptors;

namespace Project.Helpers;

public static class ServiceExtensions
{
    public static void AddRepositoryWithLogging<TRepository, TImplementation>(
        this IServiceCollection services, 
        ProxyGenerator proxyGenerator) 
        where TRepository : class 
        where TImplementation : TRepository
    {
        services.AddTransient<TRepository>(provider =>
        {
            // Identify the constructor and its parameters
            var constructorInfo = typeof(TImplementation).GetConstructors().First();
            var constructorParams = constructorInfo.GetParameters();
            var args = constructorParams.Select(p => provider.GetService(p.ParameterType)).ToArray();

            // Create the proxy with the resolved arguments
            return (TRepository)proxyGenerator.CreateClassProxy(typeof(TImplementation), args, new LoggingInterceptor(provider));
        });
    }
}

