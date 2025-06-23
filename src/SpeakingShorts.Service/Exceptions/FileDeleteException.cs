namespace SpeakingShorts.Service.Exceptions;

public class FileDeleteException : Exception
{
    public FileDeleteException(string message) : base(message) { }
    public FileDeleteException(string message, Exception innerException) : base(message, innerException) { }
} 