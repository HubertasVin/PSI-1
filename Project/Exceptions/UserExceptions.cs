[Serializable]
public class UserLoginRegisterException : Exception
{
    public new string Message { get; set; }

    public UserLoginRegisterException(string message) : base(message) {
        Message = message;
    }
}

[Serializable]
public class UserNotFoundException : Exception
{
    public new string Message { get; }

    public UserNotFoundException(string message) : base(message) {
        Message = message;
    }
}