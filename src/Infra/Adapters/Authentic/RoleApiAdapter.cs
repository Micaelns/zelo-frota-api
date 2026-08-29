using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using Infra.External.Authentic;
using Microsoft.Extensions.Options;
using Refit;
using System.Net;
using System.Text.Json;

namespace Infra.Adapters.Authentic;

public class RoleApiAdapter(IRoleApi roleApi, IOptions<AuthenticSettings> options) : IRules
{
    private readonly IRoleApi _roleApi = roleApi;
    private readonly AuthenticSettings _settings = options.Value;
    public async Task<Result<List<RoleDTO>>> RolesByUserAsync(int userId)
    {
        try
        {
            var roles = await _roleApi.RolesByUserAsync(userId, _settings.SoftwareId);
            return Result<List<RoleDTO>>.Success(roles);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            var error = "Não existe acesso para este usuário no sistema";
            return Result<List<RoleDTO>>.Failure(error, ErrorType.NotFound);
        }
        catch
        {
            return Result<List<RoleDTO>>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }
}
