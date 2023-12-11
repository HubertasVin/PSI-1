using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Project.Models;

namespace ProjectTesting.IntegrationTests;

public class CommentIntegrationTests : IDisposable
{

    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;
    
    public CommentIntegrationTests()
    {
        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task AddComment_ValidData_ReturnsOk()
    {
        // Arrange
        var response = await _client.GetAsync("/User/list");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<User>>(responseString);
        
        var response2 = await _client.GetAsync("/Subject/list");
        var responseString2 = await response2.Content.ReadAsStringAsync();
        var data2 = JsonConvert.DeserializeObject<List<Subject>>(responseString2);
        
        var response3 = await _client.GetAsync($"/Topic/list/{data2[0].id}");
        var responseString3 = await response3.Content.ReadAsStringAsync();
        var data3 = JsonConvert.DeserializeObject<List<Topic>>(responseString3);

        var comment = new Comment(userId: data[0].id, topicId: data3[0].id, message: "Test message");
        
        // Act
        var response4 = await _client.PostAsJsonAsync("/Comment/add", comment);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response4.StatusCode);
    }

    [Fact]
    public async Task AddComment_IncorrectData_ReturnsBadRequest()
    {
        // Arrange
        var comment = new Comment(userId: null, topicId: null, message: null);
        
        // Act
        var response4 = await _client.PostAsJsonAsync("/Comment/add", comment);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response4.StatusCode);
    }

    [Fact]
    public async Task RemoveComment_CorrectData_ReturnsOk()
    {
        // Arrange
        var response = await _client.GetAsync("/User/list");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<User>>(responseString);
        
        var response2 = await _client.GetAsync("/Subject/list");
        var responseString2 = await response2.Content.ReadAsStringAsync();
        var data2 = JsonConvert.DeserializeObject<List<Subject>>(responseString2);
        
        var response3 = await _client.GetAsync($"/Topic/list/{data2[0].id}");
        var responseString3 = await response3.Content.ReadAsStringAsync();
        var data3 = JsonConvert.DeserializeObject<List<Topic>>(responseString3);
        
        var response4 = await _client.GetAsync($"/Comment/get/{data3[0].id}");
        var responseString4 = await response4.Content.ReadAsStringAsync();
        var data4 = JsonConvert.DeserializeObject<List<Comment>>(responseString4);
        
        // Act
        var response5 = await _client.DeleteAsync($"/Comment/delete/{data4[0].id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response5.StatusCode);
    }
    
    [Fact]
    public async Task RemoveComment_IncorrectData_ReturnsBadRequest()
    {
        // Arrange
        var id = "123";
        
        // Act
        var response = await _client.DeleteAsync($"/Comment/delete/{id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
