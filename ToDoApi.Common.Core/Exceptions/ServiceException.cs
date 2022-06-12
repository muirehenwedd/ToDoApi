using System;

namespace ToDoApi.Common.Core.Exceptions;

public class ServiceException : Exception
{
    public ServiceException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public int ErrorCode { get; set; }
}