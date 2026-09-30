using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using Application.UseCases.Auth.Logout;
using Application.UseCases.Auth.RefreshToken;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using System.Net;
using System.Text.Json;

namespace Infra.External.Authentic.Adapters;

public class AuthenticApiAdapter(IAuthenticApi autheticApi, ILogger<AuthenticApiAdapter> logger, IOptions<AuthenticSettings> options) : IAuthentic
{
    private readonly IAuthenticApi _autheticApi = autheticApi;
    private readonly ILogger<AuthenticApiAdapter> _logger = logger;
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
        catch (Exception ex)
        {
            _logger.LogError("Erro ao acessar recurso externo LogonAsync({@logonDTO}) (Refit). {message}", logonDTO, ex.Message);
            return Result<UserAuthDTO>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }

    public async Task<Result<AuthMessageDTO>> LogoutAsync(LogoutCommand query)
    {
        try
        {
            var logout = await _autheticApi.LogoutAsync(query);
            return Result<AuthMessageDTO>.Success(logout);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
        {
            var error = "Erro interno";
            if (!string.IsNullOrEmpty(ex.Content))
            {
                error = (JsonSerializer.Deserialize<ErrorAuthDTO>(ex.Content))?.Message ?? error;
            }

            return Result<AuthMessageDTO>.Failure(error, ErrorType.AccessUnauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao acessar recurso externo LogoutAsync({@query}) (Refit). {message}", query, ex.Message);
            return Result<AuthMessageDTO>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }

    public async Task<Result<UserAuthDTO>> RefreshAsync(RefreshTokenCommand refreshTokenCommand)
    {
        try
        {
            var refresh = await _autheticApi.RefreshAsync(refreshTokenCommand);
            return Result<UserAuthDTO>.Success(refresh);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
        {
            var error = "Erro interno";
            if (!string.IsNullOrEmpty(ex.Content))
            {
                error = (JsonSerializer.Deserialize<ErrorAuthDTO>(ex.Content))?.Message ?? error;
            }

            return Result<UserAuthDTO>.Failure(error, ErrorType.AccessUnauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao acessar recurso externo RefreshAsync({@refreshTokenCommand}) (Refit). {message}", refreshTokenCommand, ex.Message);
            return Result<UserAuthDTO>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }
}
