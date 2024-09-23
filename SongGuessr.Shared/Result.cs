namespace SongGuessr.Shared;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }
}

public static class KnownResultErrors
{
    public const string Forbidden = "Forbidden";
    public const string NotFound = "NotFound";
    public const string InvalidRequest = "InvalidRequest";
    public const string Unauthorized = "Unauthorized";
}
