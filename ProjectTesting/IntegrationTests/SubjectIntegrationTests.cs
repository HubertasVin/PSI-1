using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Project.Models;
using Project.Repository;

namespace ProjectTesting.IntegrationTests;

public class SubjectIntegrationTests : IDisposable
{
    private readonly HttpClient _client;
    
    //Creating factory like this because of TestWebApplicationFactory.cs internal class
    //It does not allow itself to be passed through a constructor
    private readonly TestWebApplicationFactory _factory;

    public SubjectIntegrationTests()
    {
        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task ListSubjects_ReturnsSubjectsList()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync("/Subject/list");
        var responseString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<Subject>>(responseString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Collection((data as IEnumerable<Subject>)!,
            subject1 => Assert.Equal("Math", subject1.Name),
            subject2 => Assert.Equal("Physics", subject2.Name),
            subject3 => Assert.Equal("Chemistry", subject3.Name));
        
    }
    
    [Fact]
    public async Task GetSubject_SubjectFound_ReturnsSubject()
    {
        // Arrange
        var realSubject = new Subject("Reality");
        
        // Act
        var responseFromUpload = await _client.PostAsJsonAsync("/Subject/upload", realSubject);
        var responseStringFromUpload = await responseFromUpload.Content.ReadAsStringAsync();
        var resultFromUpload = JsonConvert.DeserializeObject<Subject>(responseStringFromUpload);
        
        var response = await _client.GetAsync($"/Subject/get/{resultFromUpload?.id}");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Subject>(responseString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Reality", result?.Name);
    }
    
    [Fact]
    public async Task GetSubject_SubjectNotFound_ReturnNotFound()
    {
        // Arrange
        var id = "123";
        
        // Act
        var response = await _client.GetAsync($"/Subject/get/{id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UploadSubject_ValidSubject_ReturnsOk()
    {
        // Arrange
        var validSubject = new Subject("Valid subject");
        
        // Act
        var response = await _client.PostAsJsonAsync("/Subject/upload", validSubject);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Subject>(responseString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Valid subject", result?.Name);
    }
    
    [Fact]
    public async Task UploadSubject_InvalidSubject_ReturnBadRequest()
    {
        // Arrange
        var invalidSubject = new Subject(null); // invalid name
        
        // Act
        var response = await _client.PostAsJsonAsync("/Subject/upload", invalidSubject);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}