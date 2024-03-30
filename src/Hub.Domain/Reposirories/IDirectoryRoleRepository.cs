using Hub.Domain.Entities;

namespace Hub.Domain.Repositories;

public interface IDirectoryRoleRepository : IBaseRepository<DirectoryRole>
{
    Task<bool> ExistsByDirectoryAndRole(string directoryId, string roleId);
}
