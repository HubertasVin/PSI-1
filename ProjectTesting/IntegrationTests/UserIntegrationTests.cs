using System.Net;
using System.Text;
using Newtonsoft.Json;
using Project.Models;

namespace ProjectTesting.IntegrationTests;

public class UserIntegrationTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;
    
    public UserIntegrationTests()
    {
        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task GetUserById_ValidId_ReturnsOk()
    {
        // Arrange
        var response = await _client.GetAsync("/User/list");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<User>>(responseString);
        
        // Act
        var response2 = await _client.GetAsync($"/User/get/{data[0].id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
    }
    
    [Fact]
    public async Task GetUserById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = "123";
        
        // Act
        var response = await _client.GetAsync($"/User/get/{id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetUserByEmail_ValidEmail_ReturnsOk()
    {
        // Arrange
        var response = await _client.GetAsync("/User/list");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<User>>(responseString);
        
        // Act
        var response2 = await _client.GetAsync($"/User/get-by-email/{data[0].Email}");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
    }
    
    [Fact]
    public async Task GetUserByEmail_InvalidEmail_ReturnsNotFound()
    {
        // Arrange
        var email = "123";
        
        // Act
        var response = await _client.GetAsync($"/User/get-by-email/{email}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task ListUsers_ReturnsOk()
    {
        // Arrange
        var response = await _client.GetAsync("/User/list");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Register_ValidUser_ReturnsOk()
    {
        // Arrange
        var newUser = new User("John", "Doe", "abc@def.com", "password123");
        var json = JsonConvert.SerializeObject(newUser);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/User/register", data);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Register_InvalidUser_ReturnsBadRequest()
    {
        // Arrange
        var userJSON = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"email\":\"}";
        var data = new StringContent(userJSON, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/User/register", data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Register_InvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var newUser = new User("John", "Doe", "abc", "password123");
        var json = JsonConvert.SerializeObject(newUser);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/User/register", data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsOk()
    {
        // Arrange
        var existingUser = new User("John", "Doe", "abcd@efgh.com", "abrakadabra");
        var json = JsonConvert.SerializeObject(existingUser);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/User/login", data);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_InvalidUser_ReturnsBadRequest()
    {
        // Arrange
        var existingUser = new User("John", "Doe", "a", "b");
        var json = JsonConvert.SerializeObject(existingUser);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/User/login", data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}