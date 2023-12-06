using System.Text.Json;
using System.Text.RegularExpressions;
using Project.Data;
using Project.Exceptions;
using Project.Models;

namespace Project.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public NoteBlendDbContext NoteBlendContext => Context as NoteBlendDbContext;
    
    public UserRepository(NoteBlendDbContext context) : base(context)
    {
        
    }

    public virtual User? GetUser(string id)
    {
        return Find(user => user.id == id).FirstOrDefault();
    }
    
    public virtual List<User> GetUserList()
    {
        return NoteBlendContext?.Users.ToList() ?? new List<User>();
    }

    public virtual User? GetUserByEmail(string email)
    {
        User? user = NoteBlendContext?.Users.FirstOrDefault(user => user.Email == email);
        return user ?? null;
    }

    public virtual bool IsEmailTaken(string email)
    {
        User? user = GetUserByEmail(email);
        return user == null;
    }

    public virtual string? CheckLogin(User existingUser)
    {
        Console.WriteLine("in check login");
        if (existingUser.Email == null || existingUser.Password == null)
            throw new UserLoginRegisterException("Email or password field is empty");
        
        User? user = GetUserByEmail(existingUser.Email);
        if (user != null && user.Password == existingUser.Password)
        {
            return user.id;
        }
        throw new UserLoginRegisterException("Either email or password is incorrect");
    }

    public virtual User? CreateUser(User newUser)
    {
        if (newUser.Email != null && !IsEmailTaken(newUser.Email))
            throw new UserLoginRegisterException("Email already exists");
        if (newUser.Email != null && !IsEmailValid(newUser.Email))
            throw new UserLoginRegisterException("Email is not valid");

        Add(newUser);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0 ? newUser : null;
    }

    public virtual bool IsEmailValid(string userEmail)
    {
        Regex regex = new(@"[\w.+-]+@\[?[\w-]+\.[\w.-]+\]?");
        return regex.IsMatch(userEmail);
    }
}