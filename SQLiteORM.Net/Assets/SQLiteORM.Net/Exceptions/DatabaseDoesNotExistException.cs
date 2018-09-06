using System;
using System.Runtime.Serialization;

public class DatabaseDoesNotExistException : Exception
{
    public DatabaseDoesNotExistException()
    {
    }

    public DatabaseDoesNotExistException(string message) : base(message)
    {
    }

    public DatabaseDoesNotExistException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected DatabaseDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
