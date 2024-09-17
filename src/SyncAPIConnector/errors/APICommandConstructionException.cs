using System;
using System.Runtime.Serialization;

namespace XApi;

[Serializable]
public class APICommandConstructionException : Exception
{
    public APICommandConstructionException()
        : base()
    {
    }

    public APICommandConstructionException(string message)
        : base(message)
    {
    }

    public APICommandConstructionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected APICommandConstructionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}