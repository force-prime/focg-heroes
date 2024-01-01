using System;
using UnityEngine;

public class Error
{
    private string _message;

    public Error(string message)
    {
        _message = message;
    }

    public Error(Exception e)
    {
        _message = e.Message;
    }

    public override string ToString()
    {
        return _message;
    }
}

public class Result<T>
{
    public Result(T value)
    {
        Value = value;
        Error = null;
    }

    public Result(Error error)
    {
        Error = error;
        Value = default;
    }

    public T? Value { get; private set; }
    public Error Error { get; private set; }

    public bool IsSuccess => Error == null;
    public bool IsError => Error != null;

    public static implicit operator T(Result<T> r) => r.Value;
    public static implicit operator Error(Result<T> r) => r.Error;
}
