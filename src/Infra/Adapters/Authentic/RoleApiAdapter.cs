using Application.Contracts.Abstractions;
using Application.Contracts.Abstractions.Cache;
using Application.DTO;
using Application.DTO.Authentic;
using Infra.Cache;
using Infra.External.Authentic;
using Microsoft.Extensions.Options;
using Refit;
using System.Net;

namespace Infra.Adapters.Authentic;

public class RoleApiAdapter(IRoleApi roleApi, IRoleCache roleCache, IUserRoleCache userRoleCache, IOptions<AuthenticSettings> options, IOptions<CacheSettings> optionsCache) : IRoles
{
    private readonly AuthenticSettings _settings = options.Value;
    private readonly CacheSettings _cacheConfig = optionsCache.Value;
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
            var roles = await _roleApi.RolesAsync(_settings.SoftwareId);
            await _roleCache.SetAsync(roles, TimeSpan.FromHours(_cacheConfig.TimeCacheRolesHours));
            return Result<List<RoleDTO>>.Success(roles);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            var error = "Não existe acessos para este sistema";
            return Result<List<RoleDTO>>.Failure(error, ErrorType.NotFound);
        }
        catch
        {
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
            var roles = await _roleApi.RolesByUserAsync(userId, _settings.SoftwareId);
            await _userRoleCache.SetAsync(userId,roles, TimeSpan.FromMinutes(_cacheConfig.TimeCacheRolesUserMinutes));
            return Result<List<RoleSimpleDTO>>.Success(roles);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            var error = "Não existe acesso para este usuário no sistema";
            return Result<List<RoleSimpleDTO>>.Failure(error, ErrorType.NotFound);
        }
        catch
        {
            return Result<List<RoleSimpleDTO>>.Failure("Servidor de autenticação indisponível temporariamente.", ErrorType.ExternalServiceUnavailable);
        }
    }
}
