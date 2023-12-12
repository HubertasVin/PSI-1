namespace Project.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string message) : base(message) { }
}