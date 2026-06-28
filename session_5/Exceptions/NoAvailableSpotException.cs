namespace Exceptions;
public class NoAvailableSpotException : Exception
{
    public NoAvailableSpotException(string message) : base(message)
    {
    }
}