namespace Project.Middlewares;

public class AllowedPathsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AllowedPathsMiddleware> _logger;
    // private readonly string[] _allowedPaths;
    private readonly string[] _allowedPaths = new string[] {"/Subject/", "/Topic/", "/User/", "/Comment/", "/Conspect/", "/chatHub"};
    
    public AllowedPathsMiddleware(RequestDelegate next, ILogger<AllowedPathsMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        // _allowedPaths = allowedPaths;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value;

        if (!isValidPath(path))
        {
            _logger.LogError("Invalid path {path}", path);
            context.Response.StatusCode = 404;
            return;
        }

        await _next(context);
    }

    private bool isValidPath(string path) =>
        _allowedPaths.Any(allowedPaths => path.StartsWith(allowedPaths, StringComparison.OrdinalIgnoreCase));
}