using ResultErrorType = Application.DTO.ErrorType;

namespace Application.DTO;

public class Result<T>
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public ErrorType? ErrorType { get; }
    public T? Value { get; } = default;

    private Result(bool success, T value, string? error, ErrorType? errorType) { 
        IsSuccess = success;
        Value = value;
        Error = error;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null, null);
    }

    public static Result<T> Failure(string error, ErrorType? errorType = ResultErrorType.Validation)
    {
        return new Result<T>(false, default, error, errorType);
    }
}
