using System.Text.Json;
using Project.Data;
using Project.Models;

namespace Project.Repository;

public class SubjectRepository : Repository<Subject>
{

    public NoteBlendDbContext NoteBlendContext => Context as NoteBlendDbContext;
    
    public SubjectRepository(NoteBlendDbContext context) : base (context)
    {
        
    }
    
    public List<Subject> GetSubjectsList()
    {
        return NoteBlendContext.Subjects.ToList();
    }
    
    public Subject GetSubject(string id)
    {
        return NoteBlendContext.Subjects.Find(id);
    }

    public Subject? CreateSubject(Subject newSubject)
    {
        Add(newSubject);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0 ? newSubject : null;
    }
}