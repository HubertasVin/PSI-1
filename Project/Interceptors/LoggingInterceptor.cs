using Castle.DynamicProxy;
using Serilog;

namespace Project.Interceptors;

public class LoggingInterceptor : IInterceptor
{
    private readonly IServiceProvider _serviceProvider;

    public LoggingInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Intercept(IInvocation invocation)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<LoggingInterceptor>>();

        logger.LogInformation(
            $"Calling method {invocation.Method.Name} with arguments {string.Join(", ", invocation.Arguments)}");
        invocation.Proceed();
        logger.LogInformation($"Method {invocation.Method.Name} returned {invocation.ReturnValue}");
    }
}