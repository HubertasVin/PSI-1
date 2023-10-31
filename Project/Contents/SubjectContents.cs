using System.Text.Json;
using Project.Chat;
using Project.Models;

namespace Project.Contents;

public class SubjectContents
{
    private List<Subject> _subjectsList;
    private string _filePath = "src/SubjectData.json";
    
    public SubjectContents()
    {
        InitContents();
    }

    public void InitContents()
    {
        string json = File.ReadAllText(_filePath);
        _subjectsList = JsonSerializer.Deserialize<List<Subject>>(json);
        Console.WriteLine(_subjectsList);
    }
    
    public List<Subject> GetSubjectsList()
    {
        return _subjectsList;
    }
    
    public Subject GetSubject(string id)
    {
        return _subjectsList.Find(subject => subject.id == id);
    }

    public Subject? CreateSubject(JsonElement req)
    {
        if (!req.TryGetProperty("subjectName", out var subjectNameProperty))
            return null;
    
        string? subjectName = subjectNameProperty.GetString();
        Console.WriteLine(subjectName);
        Subject newSubject = new Subject(subjectName);
        _subjectsList.Add(newSubject);
        JSONParser.WriteToJSON(_filePath, _subjectsList);
        return newSubject;
    }
}