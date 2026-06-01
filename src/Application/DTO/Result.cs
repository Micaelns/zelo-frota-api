using ResultErrorType = Application.DTO.ErrorType;

namespace Application.DTO;

public class Result<T>
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public ErrorType? ErrorType { get; }
    public T? Value { get; } = default;
    public Pagination? Pagination { get; }

    private Result(bool success, T value, string? error, ErrorType? errorType, Pagination? pagination) { 
        IsSuccess = success;
        Value = value;
        Error = error;
        ErrorType = errorType;
        Pagination = pagination;
    }

        public static Result<T> Success()
        {
            return new Result<T>(true, default, null, null, null);
        }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null, null, null);
    }

    public static Result<T> Success(T value, Pagination pagination)
    {
        return new Result<T>(true, value, null, null, pagination);
    }

    public static Result<T> Failure(string error, ErrorType? errorType = ResultErrorType.Validation)
    {
        return new Result<T>(false, default, error, errorType, null);
    }
}
