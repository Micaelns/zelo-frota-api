using Application.DTO;

namespace IntegrationsTests;

public class ApiResponse<T>(bool isSuccess, T value, string? error, ErrorType? errorType, Pagination? pagination)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string? Error { get; set; } = error;
    public ErrorType? ErrorType { get; set; } = errorType;
    public T? Value { get; set; } = value;
    public Pagination? Pagination { get; set; } = pagination;
}
