namespace FixFlow.Application.Common;

public enum ErrorType
{
    None = 0,
    Validation = 1,
    NotFound = 2,
    Forbidden = 3,
    Conflict = 4
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public ErrorType ErrorType { get; }

    protected Result(bool isSuccess, string? error, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
    }

    public static Result Success()
    {
        return new Result(true, null, ErrorType.None);
    }

    public static Result Failure(string error)
    {
        return new Result(false, error, ErrorType.Validation);
    }

    public static Result NotFound(string error)
    {
        return new Result(false, error, ErrorType.NotFound);
    }

    public static Result Forbidden(string error)
    {
        return new Result(false, error, ErrorType.Forbidden);
    }

    public static Result Conflict(string error)
    {
        return new Result(false, error, ErrorType.Conflict);
    }
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string? error, ErrorType errorType)
        : base(isSuccess, error, errorType)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null, ErrorType.None);
    }

    public static new Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error, ErrorType.Validation);
    }

    public static new Result<T> NotFound(string error)
    {
        return new Result<T>(false, default, error, ErrorType.NotFound);
    }

    public static new Result<T> Forbidden(string error)
    {
        return new Result<T>(false, default, error, ErrorType.Forbidden);
    }

    public static new Result<T> Conflict(string error)
    {
        return new Result<T>(false, default, error, ErrorType.Conflict);
    }
}