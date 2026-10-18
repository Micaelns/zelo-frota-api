using Application.DTO;

namespace Api.Helper;

public static class DecodeHelper
{
    public static int ErrorTypeToStatusCode(ErrorType? validation)
    {
        return validation switch
        {
            ErrorType.Validation => 400,
            ErrorType.AccessUnauthorized => 401,
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.ExternalServiceUnavailable => 503,
            _ => 500
        };
    }
}
