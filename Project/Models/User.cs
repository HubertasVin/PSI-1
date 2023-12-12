namespace Project.Models;

public class User : BaseModel
{
    private string? _name;
    private string? _surname;
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string? Name
    {
        get => _name;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException("Name cannot be null");
            }
            if (value.Length < 3)
            {
                throw new ArgumentException("Name must be at least 3 characters long");
            }
            _name = value;
        }
    }
    
    public string? Surname
    {
        get => _surname;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException("Surname cannot be null");
            }
            if (value.Length < 3)
            {
                throw new ArgumentException("Surname must be at least 3 characters long");
            }
            _surname = value;
        }
    }

    public User(string name, string surname, string email, string password)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
    }
}