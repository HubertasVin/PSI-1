using System.Net;
using Newtonsoft.Json;
using Project.Models;

namespace ProjectTesting.IntegrationTests;

public class TopicIntegrationTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;
    
    public TopicIntegrationTests()
    {
        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task ListTopics_ValidSubjectId_ReturnsTopicsList()
    {
        // Arrange
        var responseSubjects = await _client.GetAsync("/Subject/list");
        var responseStringSubjects = await responseSubjects.Content.ReadAsStringAsync();
        var listOfSubjects = JsonConvert.DeserializeObject<List<Subject>>(responseStringSubjects);
        
        // Act
        var response = await _client.GetAsync($"/Topic/list/{listOfSubjects[0].id}");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<Topic>>(responseString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Collection((data as IEnumerable<Topic>)!,
            topic1 => Assert.Equal("Algebra", topic1.Name),
            topic2 => Assert.Equal("Geometry", topic2.Name));
    }
    
    [Fact]
    public async Task ListTopics_InvalidSubjectId_ReturnsOkEmptyList()
    {
        // Arrange
        var id = "123";
        
        // Act
        var response = await _client.GetAsync($"/Topic/list/{id}");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<Topic>>(responseString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty(data!);
    }
    
    [Fact]
    public async Task GetTopicById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = "123";
        
        // Act
        var response = await _client.GetAsync($"/Topic/get/{id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}