using Hub.Domain.Entities;

namespace Hub.Domain.Repositories;

public interface IFileRoleRepository : IBaseRepository<FileRole>
{
    Task<bool> ExistsByFileAndRole(string fileId, string roleId);
}
