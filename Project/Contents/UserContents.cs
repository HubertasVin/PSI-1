using System.Net.Mail;
using System.Text.Json;
using System.Text.RegularExpressions;
using Project.Chat;
using Project.Data;
using Project.Models;

namespace Project.Contents;

public class UserContents : Contents<User>
{
    public NoteBlendDbContext? NoteBlendContext => Context as NoteBlendDbContext;
    // private List<Topic> _topicList;
    // private string _filepath = "src/TopicData.json";
    // private SubjectContents _subjectContents = new SubjectContents();
    
    public UserContents(NoteBlendDbContext context) : base(context)
    {
        
    }
    
    public List<User> GetUserList()
    {
        return NoteBlendContext?.Users.ToList() ?? new List<User>();
    }

    public User? GetUserByEmail(string email)
    {
        User? user = NoteBlendContext?.Users.FirstOrDefault(user => user.Email == email);
        return user ?? null;
    }

    public bool IsEmailTaken(string email)
    {
        User? user = GetUserByEmail(email);
        return user == null;
    }

    public String? CheckLogin(JsonElement req)
    {
        if (!req.TryGetProperty("userEmail", out var userEmailProperty) ||
            !req.TryGetProperty("userPassword", out var userPasswordProperty))
            return null;
        
        string? userEmail = userEmailProperty.GetString();
        string? userPassword = userPasswordProperty.GetString();
        
        Console.WriteLine("User email: " + userEmail);
        Console.WriteLine("User password: " + userPassword);

        if (userEmail == null || userPassword == null)
        {
            return null;
        }
        
        User? user = GetUserByEmail(userEmail);
        if (user != null && user.Password == userPassword)
        {
            return user.id;
        }
        return null;
    }

    public User? CreateUser(JsonElement req)
    {
        if (!req.TryGetProperty("userName", out var userNameProperty) ||
            !req.TryGetProperty("userSurname", out var userSurnameProperty) ||
            !req.TryGetProperty("userEmail", out var userEmailProperty) ||
            !req.TryGetProperty("userPassword", out var userPasswordProperty))
            return null;
        
        string? userName = userNameProperty.GetString();
        string? userSurname = userSurnameProperty.GetString();
        string? userEmail = userEmailProperty.GetString();
        string? userPassword = userPasswordProperty.GetString();
        
        Console.WriteLine("User name: " + userName);
        Console.WriteLine("User surname: " + userSurname);
        Console.WriteLine("User email: " + userEmail);
        Console.WriteLine("User password: " + userPassword);

        Console.WriteLine($"Creating new user named: {userName}");
        Console.WriteLine($"User surname: {userSurname}");
        Console.WriteLine($"User email: {userEmail}");
        Console.WriteLine($"User password: {userPassword}");
        User newUser = new User(userName ?? "", userSurname ?? "", userEmail ?? "", userPassword ?? "");
        Add(newUser);
        int changes = 0;
        if (NoteBlendContext != null)
        {
            changes = NoteBlendContext.SaveChanges();
        }
        // JSONParser.WriteToJSON(_filepath, _topicList);
        return changes > 0 ? newUser : null;
    }

    internal string? IsEmailTaken(object userEmail)
    {
        throw new NotImplementedException();
    }

    internal static bool IsEmailValid(string userEmail)
    {
        Regex regex = new(@"[\w.+-]+@\[?[\w-]+\.[\w.-]+\]?");
        return regex.IsMatch(userEmail);
    }
}