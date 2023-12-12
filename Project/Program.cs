using Castle.DynamicProxy;
using Project.Hubs;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Project.Data;
using Project.Interceptors;
using Project.Repository;
using Project.Services;
using Project.Helpers;
using Project.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var proxyGenerator = new ProxyGenerator();

var logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Adding PostgreSQL as DbContext
builder.Services.AddDbContext<NoteBlendDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
            builder
                .WithOrigins("https://localhost:44465") // Updated with your React app's URL
                .AllowAnyMethod()
                .AllowAnyHeader()
    );
});

// Dependency injection for repositories
builder.Services.AddRepositoryWithLogging<ISubjectRepository, SubjectRepository>(proxyGenerator);
builder.Services.AddRepositoryWithLogging<ITopicRepository, TopicRepository>(proxyGenerator);
// builder.Services.AddTransient<ITopicRepository, TopicRepository>();
builder.Services.AddRepositoryWithLogging<IUserRepository, UserRepository>(proxyGenerator);
builder.Services.AddRepositoryWithLogging<ICommentRepository, CommentRepository>(proxyGenerator);
builder.Services.AddRepositoryWithLogging<IConspectRepository, ConspectRepository>(proxyGenerator);

// Dependency injection for services
builder.Services.AddTransient<ChatService>();

// Dependency injection for Interceptors

var app = builder.Build();
var env = builder.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseMiddleware<AllowedPathsMiddleware>();

app.UseAuthorization();
app.UseCors();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<ChatHub>("chatHub");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(env.ContentRootPath, "ClientApp", "public")),
    RequestPath = "/public"
});

app.Run();