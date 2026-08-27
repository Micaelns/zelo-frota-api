using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using Infra.External.Authentic;
using Microsoft.Extensions.Options;
using Refit;
using System.Net;
using System.Text.Json;

namespace Infra.Adapters.Authentic;

public class AuthenticApiAdapter(IAuthenticApi autheticApi, IOptions<AuthenticSettings> options) : IAuthentic
{
    private readonly IAuthenticApi _autheticApi = autheticApi;
    private readonly AuthenticSettings _settings = options.Value;

    public async Task<Result<UserAuthDTO>> LogonAsync(LogonRequestDto logonDTO)
    {
        try
        {
            logonDTO.SoftwareId = _settings.SoftwareId;
            var logon = await _autheticApi.LogonAsync(logonDTO);
            return Result<UserAuthDTO>.Success(logon);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            var error = "Credenciais inválidas"; 
            if (!string.IsNullOrEmpty(ex.Content))
            {
                error = (JsonSerializer.Deserialize<ErrorAuthDTO>(ex.Content))?.Message ?? error;
            }
             
            return Result<UserAuthDTO>.Failure(error, ErrorType.AccessUnauthorized);
        }
        catch
        {
            return Result<UserAuthDTO>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }
}
