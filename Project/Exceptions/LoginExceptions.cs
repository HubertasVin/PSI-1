[Serializable]
public class UserLoginRegisterException : Exception
{
    public new string Message { get; set; }

    public UserLoginRegisterException(string message) : base(message) {
        this.Message = message;
    }
}