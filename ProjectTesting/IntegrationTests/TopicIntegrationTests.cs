using System.Net;
using System.Text;
using System.Text.Json;
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
    public async Task ListTopics_InvalidSubjectId_ReturnsNotFound()
    {
        // Arrange
        var id = "123";
        
        // Act
        var response = await _client.GetAsync($"/Topic/list/{id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTopicById_ValidId_ReturnsOk()
    {
        // Arrange
        var responseSubjects = await _client.GetAsync("/Subject/list");
        var responseStringSubjects = await responseSubjects.Content.ReadAsStringAsync();
        var listOfSubjects = JsonConvert.DeserializeObject<List<Subject>>(responseStringSubjects);
        
        var responseTopics = await _client.GetAsync($"/Topic/list/{listOfSubjects[0].id}");
        var responseStringTopics = await responseTopics.Content.ReadAsStringAsync();
        var listOfTopics = JsonConvert.DeserializeObject<List<Topic>>(responseStringTopics);
        
        // Act
        var response = await _client.GetAsync($"/Topic/get/{listOfTopics[0].id}");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<Topic>(responseString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Algebra", data?.Name);
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
    
    [Fact]
    public async Task UploadTopic_ValidTopic_ReturnsOk()
    {
        // Arrange
        var responseSubjects = await _client.GetAsync("/Subject/list");
        var responseStringSubjects = await responseSubjects.Content.ReadAsStringAsync();
        var listOfSubjects = JsonConvert.DeserializeObject<List<Subject>>(responseStringSubjects);
        
        var jsonElement = JsonDocument.Parse("{\"topicName\":\"Mechanics\",\"subjectId\":\"" + listOfSubjects[0].id + "\"}").RootElement;
        
        // Act
        var response = await _client.PostAsync("/Topic/upload", new StringContent(jsonElement.ToString(), Encoding.UTF8, "application/json"));
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<Topic>(responseString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Mechanics", data?.Name);
    }
    
    [Fact]
    public async Task UploadTopic_InvalidTopic_ReturnsBadRequest()
    {
        // Arrange
        var jsonElement = JsonDocument.Parse("{\"topicName\":\"Mechanics\"}").RootElement;
        
        // Act
        var response = await _client.PostAsync("/Topic/upload", new StringContent(jsonElement.ToString(), Encoding.UTF8, "application/json"));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}