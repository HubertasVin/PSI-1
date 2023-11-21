namespace Project.Exceptions;

public class ConspectAlreadyExistsException : Exception
{
    public new string Message { get; }

    public ConspectAlreadyExistsException(string message) : base(message) {
        Message = message;
    }
}

public class ConspectNotFoundException : Exception
{
    public new string Message { get; }

    public ConspectNotFoundException(string message) : base(message) {
        Message = message;
    }
}