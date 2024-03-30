using Hub.Domain.Entities;

namespace Hub.Domain.Repositories;

public interface IDirectoryFlagRepository : IBaseRepository<DirectoryFlag>
{
    Task<bool> ExistsByDirectoryAndFlag(string directoryId, string flagId);
}
