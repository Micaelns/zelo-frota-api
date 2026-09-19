namespace Application.Contracts.Abstractions;

public interface IPermissionAuthorization
    {
        Task<bool> HasPermissionAsync(
            int userId,
            string permission);
    }
