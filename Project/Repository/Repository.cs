using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Repository;

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
        return Context.Set<T>().Find(id) ?? throw new UserNotFoundException("User not found");
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
