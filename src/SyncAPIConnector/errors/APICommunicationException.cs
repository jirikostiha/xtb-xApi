using System;
using System.Runtime.Serialization;

namespace xAPI;

[Serializable]
public class APICommunicationException : Exception
{
    public APICommunicationException()
        : base()
    {
    }

    public APICommunicationException(string message)
        : base(message)
    {
    }

    public APICommunicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected APICommunicationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}