using Project.Models;

namespace Project.Repository;

public interface IUserRepository
{
    public User? GetUser(string id);
    public List<User> GetUserList();
    public User? GetUserByEmail(string email);
    public bool IsEmailTaken(string email);
    public string? CheckLogin(User existingUser);
    public User? CreateUser(User newUser);
    public bool IsEmailValid(string userEmail);
}