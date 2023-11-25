using Project.Models;

namespace Project.Repository;

public interface ISubjectRepository
{
    public List<Subject> GetSubjectsList();
    public Subject GetSubject(string id);
    public Subject? CreateSubject(Subject newSubject);
}