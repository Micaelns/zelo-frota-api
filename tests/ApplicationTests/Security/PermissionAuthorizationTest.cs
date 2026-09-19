using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using Application.Security;
using Moq;

namespace ApplicationTests.Security;

public class PermissionAuthorizationTest
{
    public readonly Mock<IRoles> _mockRoleService;

    public PermissionAuthorizationTest() => _mockRoleService = new Mock<IRoles>();
     
    [Fact]
    public async Task HasPermissionAsync_UserInvalidOrwithoutRoles_ReturnFalse()
    {
        _mockRoleService.Setup(service => service.RolesByUserAsync(It.IsAny<int>()))
            .ReturnsAsync(Result<List<RoleSimpleDTO>>.Success([]));

        var permissionAuth= new PermissionAuthorization(_mockRoleService.Object);
        var result = await permissionAuth.HasPermissionAsync(It.IsAny<int>(), It.IsAny<string>());

        Assert.False(result);
    }

    [Fact]
    public async Task HasPermissionAsync_SoftwareWithoutRoles_ReturnFalse()
    {
        var listaRolesUser = new List<RoleSimpleDTO>(){
            new() { Name = "role-1" }
        };
        _mockRoleService.Setup(service => service.RolesByUserAsync(It.IsAny<int>()))
            .ReturnsAsync(Result<List<RoleSimpleDTO>>.Success(listaRolesUser));

        _mockRoleService.Setup(service => service.RolesAsync())
            .ReturnsAsync(Result<List<RoleDTO>>.Success([]));

        var permissionAuth = new PermissionAuthorization(_mockRoleService.Object);
        var result = await permissionAuth.HasPermissionAsync(It.IsAny<int>(), It.IsAny<string>());

        Assert.False(result);
    }

    [Fact]
    public async Task HasPermissionAsync_UserWithRolesButNoPermission_ReturnFalse()
    {
        var permission = "permission-teste";
        var listaRolesUser = new List<RoleSimpleDTO>(){
            new() { Name = "role-1" },
            new() { Name = "role-2" }
        };
        _mockRoleService.Setup(service => service.RolesByUserAsync(It.IsAny<int>()))
            .ReturnsAsync(Result<List<RoleSimpleDTO>>.Success(listaRolesUser));
        
        var listaAllRoles = new List<RoleDTO>(){
            new() { Name = "role-1", Permissions = ["permission012345689", "permission9876543210"] },
            new() { Name = "role-2", Permissions = ["permission0123456", "permission6543210"] }
        };
        _mockRoleService.Setup(service => service.RolesAsync())
            .ReturnsAsync(Result<List<RoleDTO>>.Success(listaAllRoles));

        var permissionAuth = new PermissionAuthorization(_mockRoleService.Object);
        var result = await permissionAuth.HasPermissionAsync(It.IsAny<int>(), permission);

        Assert.False(result);
    }

    [Fact]
    public async Task HasPermissionAsync_UserWithRolesWithPermission_ReturnTrue()
    {
        var permission = "permission-teste";
        var listaRolesUser = new List<RoleSimpleDTO>(){
            new() { Name = "role-1" },
            new() { Name = "role-3" }
        };
        _mockRoleService.Setup(service => service.RolesByUserAsync(It.IsAny<int>()))
            .ReturnsAsync(Result<List<RoleSimpleDTO>>.Success(listaRolesUser));

        var listaAllRoles = new List<RoleDTO>(){
            new() { Name = "role-1", Permissions = ["permission012345689", "permission9876543210"] },
            new() { Name = "role-2", Permissions = ["permission0123456", "permission6543210"] },
            new() { Name = "role-3", Permissions = ["permission0", permission] }

        };
        _mockRoleService.Setup(service => service.RolesAsync())
            .ReturnsAsync(Result<List<RoleDTO>>.Success(listaAllRoles));

        var permissionAuth = new PermissionAuthorization(_mockRoleService.Object);
        var result = await permissionAuth.HasPermissionAsync(It.IsAny<int>(), permission);

        Assert.True(result);
    }
}
