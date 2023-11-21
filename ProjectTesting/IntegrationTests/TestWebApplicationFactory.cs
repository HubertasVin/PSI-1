using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Data;
using Project.Models;

namespace ProjectTesting.IntegrationTests;

internal class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<NoteBlendDbContext>)
            );

            services.Remove(descriptor);
            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<NoteBlendDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<NoteBlendDbContext>();

            db.Database.EnsureCreated();

            User user1 = new User("John", "Doe", "abcd@efgh.com", "abrakadabra");
            db.Users.Add(user1);
            
            Subject subject1 = new Subject("Math");
            db.Subjects.Add(subject1);
            Subject subject2 = new Subject("Physics");
            db.Subjects.Add(subject2);
            Subject subject3 = new Subject("Chemistry");
            db.Subjects.Add(subject3);
            
            Topic topic1 = new Topic("Algebra", subject1);
            db.Topics.Add(topic1);
            Topic topic2 = new Topic("Geometry", subject1);
            db.Topics.Add(topic2);
            Topic topic3 = new Topic("Mechanics", subject2);
            db.Topics.Add(topic3);
            Topic topic4 = new Topic("Thermodynamics", subject2);
            db.Topics.Add(topic4);
            Topic topic5 = new Topic("Organic Chemistry", subject3);
            db.Topics.Add(topic5);
            Topic topic6 = new Topic("Inorganic Chemistry", subject3);
            db.Topics.Add(topic6);

            db.SaveChanges();
        });
    }
}