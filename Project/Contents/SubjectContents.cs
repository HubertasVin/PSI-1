using System.Text.Json;
using Project.Data;
using Project.Models;

namespace Project.Contents;

public class SubjectContents : Contents<Subject>
{

    public NoteBlendDbContext NoteBlendContext => Context as NoteBlendDbContext;
    // private List<Subject> _subjectsList;
    // private string _filePath = "src/SubjectData.json";
    
    public SubjectContents(NoteBlendDbContext context) : base (context)
    {
        
    }

    // public void InitContents()
    // {
    //     string json = File.ReadAllText(_filePath);
    //     _subjectsList = JsonSerializer.Deserialize<List<Subject>>(json);
    //     Console.WriteLine(_subjectsList);
    // }
    
    public List<Subject> GetSubjectsList()
    {
        return NoteBlendContext.Subjects.ToList();
    }
    
    public Subject GetSubject(string id)
    {
        return NoteBlendContext.Subjects.Find(id);
    }

    public Subject? CreateSubject(JsonElement req)
    {
        if (!req.TryGetProperty("subjectName", out var subjectNameProperty))
            return null;
    
        string? subjectName = subjectNameProperty.GetString();
        Console.WriteLine(subjectName);
        Subject newSubject = new Subject(subjectName);
        Add(newSubject);
        // JSONParser.WriteToJSON(_filePath, _subjectsList);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0 ? newSubject : null;
    }
}