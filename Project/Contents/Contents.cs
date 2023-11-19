using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Contents;

public class Contents<T>
    where T : BaseModel
{
    protected readonly DbContext Context;

    public Contents(NoteBlendDbContext context)
    {
        Context = context;
    }

    public T? Get(string id)
    {
        return Context.Set<T>().Find(id);
    }

    public List<T> GetAll()
    {
        return Context.Set<T>().ToList();
    }

    public List<T> Find(Expression<Func<T, bool>> pred)
    {
        return Context.Set<T>().Where(pred).ToList();
    }

    public void Add(T item)
    {
        Context.Set<T>().Add(item);
    }

    public void Remove(T item)
    {
        Context.Set<T>().Remove(item);
    }
}
