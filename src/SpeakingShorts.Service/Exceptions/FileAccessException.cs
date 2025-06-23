namespace SpeakingShorts.Service.Exceptions;

public class FileAccessException : Exception
{
    public FileAccessException(string message) : base(message) { }
    public FileAccessException(string message, Exception innerException) : base(message, innerException) { }
} 