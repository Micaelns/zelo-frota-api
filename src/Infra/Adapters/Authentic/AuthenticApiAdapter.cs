using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using Infra.External.Authentic;
using Refit;
using System.Net;

namespace Infra.Adapters.Authentic;

public class AuthenticApiAdapter(IAuthenticApi autheticApi): IAuthentic
{
    private readonly IAuthenticApi _autheticApi = autheticApi;

    public async Task<Result<UserAuthDTO>> LogonAsync(LogonRequestDto logonDTO)
    {
        try
        {
            var logon = await _autheticApi.LogonAsync(logonDTO);
            return Result<UserAuthDTO>.Success(logon);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result<UserAuthDTO>.Failure("Usuário e/ou senha inválido(s).", ErrorType.Validation);
        }
        catch
        {
            return Result<UserAuthDTO>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }
}
