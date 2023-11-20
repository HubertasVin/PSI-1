namespace Project.Exceptions;

public class UserLoginRegisterException : Exception
{
    public new string Message { get; }

    public UserLoginRegisterException(string message) : base(message) {
        Message = message;
    }
}

public class UserNotFoundException : Exception
{
    public new string Message { get; }

    public UserNotFoundException(string message) : base(message) {
        Message = message;
    }
}