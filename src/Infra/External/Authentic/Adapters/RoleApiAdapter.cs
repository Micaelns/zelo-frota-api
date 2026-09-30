using Application.Contracts.Abstractions;
using Application.Contracts.Abstractions.Cache;
using Application.DTO;
using Application.DTO.Authentic;
using Infra.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using System.Net;

namespace Infra.External.Authentic.Adapters;

public class RoleApiAdapter(IRoleApi roleApi, IRoleCache roleCache, ILogger<RoleApiAdapter> logger, IUserRoleCache userRoleCache, IOptions<AuthenticSettings> options, IOptions<CacheSettings> optionsCache) : IRoles
{
    private readonly AuthenticSettings _settings = options.Value;
    private readonly CacheSettings _cacheConfig = optionsCache.Value;
    private readonly ILogger<RoleApiAdapter> _logger = logger;
    private readonly IRoleApi _roleApi = roleApi;
    private readonly IRoleCache _roleCache = roleCache;
    private readonly IUserRoleCache _userRoleCache = userRoleCache;

    public async Task<Result<List<RoleDTO>>> RolesAsync()
    {
        try
        {
            var dataCache = await _roleCache.GetAsync();
            if (dataCache.Count() > 0)
            {
                return Result<List<RoleDTO>>.Success([..dataCache]);
            }
            _logger.LogInformation("Consulta de dados do Roles via Refit.");
            var roles = await _roleApi.RolesAsync(_settings.SoftwareId);
            await _roleCache.SetAsync(roles, TimeSpan.FromHours(_cacheConfig.TimeCacheRolesHours));
            return Result<List<RoleDTO>>.Success(roles);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            var error = "Não existe acessos para este sistema";
            return Result<List<RoleDTO>>.Failure(error, ErrorType.NotFound);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError("Erro ao acessar recurso externo RolesAsync({@SoftwareId}) (Refit) , foi retornado o status code {Unauthorized}", _settings.SoftwareId, HttpStatusCode.Unauthorized);
            var error = "Não existe usuário autenticado no sistema sistema";
            return Result<List<RoleDTO>>.Failure(error, ErrorType.AccessUnauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao acessar recurso externo RolesAsync({@SoftwareId}) (Refit). {message}", _settings.SoftwareId, ex.Message);
            return Result<List<RoleDTO>>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }

    public async Task<Result<List<RoleSimpleDTO>>> RolesByUserAsync(int userId)
    {
        try
        {
            var dataCache = await _userRoleCache.GetAsync(userId);
            if (dataCache.Count() > 0)
            {
                return Result<List<RoleSimpleDTO>>.Success([..dataCache]);
            }
            var roles = await _roleApi.RolesByUserAsync(_settings.SoftwareId);
            _logger.LogInformation("Consulta de dados do RolesByUser via Refit.");
            await _userRoleCache.SetAsync(userId,roles, TimeSpan.FromMinutes(_cacheConfig.TimeCacheRolesUserMinutes));
            return Result<List<RoleSimpleDTO>>.Success(roles);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            var error = "Não existe acesso para este usuário no sistema";
            return Result<List<RoleSimpleDTO>>.Failure(error, ErrorType.NotFound);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError("Erro ao acessar recurso externo RolesByUserAsync({@userId}, {@SoftwareId}) (Refit) , foi retornado o status code {Unauthorized}", userId, _settings.SoftwareId, HttpStatusCode.Unauthorized);
            var error = "Não existe usuário autenticado no sistema sistema";
            return Result<List<RoleSimpleDTO>>.Failure(error, ErrorType.AccessUnauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao acessar recurso externo RolesByUserAsync({@userId}, {@SoftwareId}) (Refit). {@message}", userId, _settings.SoftwareId, ex.Message);
            return Result<List<RoleSimpleDTO>>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }
}
