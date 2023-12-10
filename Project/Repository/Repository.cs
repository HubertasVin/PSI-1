using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Exceptions;
using Project.Models;

namespace Project.Repository;
// TODO: Interface
public class Repository<T>
    where T : BaseModel
{
    protected readonly DbContext Context;

    public Repository(NoteBlendDbContext context)
    {
        Context = context;
    }

    public T? Get(string id)
    {
        return Context.Set<T>().Find(id) ?? throw new ObjectNotFoundException("Object not found");
    }

    public List<T> Find(Expression<Func<T, bool>> pred)
    {
        var result = Context.Set<T>().Where(pred).ToList();
        if (result.Count == 0)
            throw new ObjectNotFoundException("Object not found");
        return result;
        // return Context.Set<T>().Where(pred).ToList();
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
